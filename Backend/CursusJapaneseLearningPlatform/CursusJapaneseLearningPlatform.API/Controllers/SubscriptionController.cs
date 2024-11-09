using CursusJapaneseLearningPlatform.Service.BusinessModels.SubcriptionModels;
using CursusJapaneseLearningPlatform.Service.Implementations;
using CursusJapaneseLearningPlatform.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CursusJapaneseLearningPlatform.API.Controllers
{
    /// <summary>
    /// Manages operations related to subscriptions.
    /// </summary>
    [ApiController]
    [Route("api/subscriptions")]
    [Authorize]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        /// <summary>
        /// Constructor for SubscriptionController.
        /// </summary>
        /// <param name="subscriptionService">Service for handling subscription operations.</param>
        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        /// <summary>
        /// Creates a new subscription for the authenticated user.
        /// </summary>
        /// <param name="request">Subscription details.</param>
        [HttpPost]
        [Authorize(Roles = "Learner")]
        public async Task<IActionResult> CreateSubscription([FromBody] SubscriptionRequestModel request, CancellationToken cancellationToken)
        {
            var idClaim = User.FindFirst("Id");
            if (idClaim == null || string.IsNullOrEmpty(idClaim.Value))
            {
                // Handle the case where the claim is missing or its value is null/empty
                return Unauthorized("User is not authenticated or does not have an Id claim.");
            }

            if (!Guid.TryParse(idClaim.Value, out var userId))
            {
                return BadRequest("The Id claim is not a valid GUID.");
            }

            var response = await _subscriptionService.CreateSubscription(request, userId, cancellationToken); // Pass the cancellation token

            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Completes a subscription using payment information.
        /// </summary>
        /// <param name="paymentId">Payment identifier.</param>
        /// <param name="payerId">Payer identifier.</param>
        /// <param name="token">Transaction token.</param>
        [HttpGet("complete")]
        [Authorize(Roles = "Learner")]
        public async Task<IActionResult> CompleteSubscription([FromQuery] string paymentId, [FromQuery] string payerId, [FromQuery] string token, CancellationToken cancellationToken)
        {
            var response = await _subscriptionService.CompleteSubscription(paymentId, payerId, token, cancellationToken); // Pass the cancellation token
            return StatusCode(response.StatusCode, response);
        }
    }
}
