using AutoMapper;
using CursusJapaneseLearningPlatform.Repository.Entities;
using CursusJapaneseLearningPlatform.Repository.Interfaces;
using CursusJapaneseLearningPlatform.Service.BusinessModels.SubcriptionModels;
using CursusJapaneseLearningPlatform.Service.Commons.BaseResponses;
using CursusJapaneseLearningPlatform.Service.Commons.Exceptions;
using CursusJapaneseLearningPlatform.Service.Commons.Implementations;
using CursusJapaneseLearningPlatform.Service.Commons.Interfaces;
using CursusJapaneseLearningPlatform.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursusJapaneseLearningPlatform.Service.Implementations
{

    // Services/SubscriptionService.cs
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPayPalClient _paypalClient;
        private readonly IMapper _mapper;

        public SubscriptionService(IUnitOfWork unitOfWork, IPayPalClient paypalClient, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _paypalClient = paypalClient;
            _mapper = mapper;
        }

        public async Task<BaseResponseModel<string>> CreateSubscription(SubscriptionRequestModel request, Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var package = await _unitOfWork.PackageRepository.GetByIdAsync(request.PackageId);
                var hasActiveSubscription = await _unitOfWork.SubscriptionRepository.HasActiveSubscriptionAsync(userId);
                if (package == null)
                {
                    throw new CustomException(StatusCodes.Status404NotFound,
                        ResponseCodeConstants.NOT_FOUND);
                }
                if (hasActiveSubscription)
                {
                    throw new CustomException(
                        StatusCodes.Status400BadRequest,
                        "User  already has an active subscription. Please wait until the current subscription expires.");
                }

                // Create PayPal payment
                var payment = await _paypalClient.CreatePayment(
                    package.Price,
                    "USD",
                    "sale",
                    $"Subscription for {package.PlanName}",
                    request.ReturnUrl,
                    request.CancelUrl,
                    cancellationToken // Pass the cancellation token
                );

                var subscription = new Subscription
                {
                    UserId = userId,
                    PackageId = package.Id,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddMonths(package.Period),
                    Amount = package.Price,
                    PaymentStatus = "Pending",
                    PaymentId = payment.Id,
                    TokenLimit = 1000,
                    IsActive = false,
                    CreatedTime = DateTime.UtcNow,
                    LastUpdatedTime = DateTime.UtcNow,
                    CreatedBy = userId.ToString(),
                    LastUpdatedBy = userId.ToString(),
                    IsDelete = false
                };

                await _unitOfWork.SubscriptionRepository.AddAsync(subscription);
                await _unitOfWork.SaveAsync();

                var approvalUrl = payment.Links.First(l => l.Rel == "approval_url").Href;
                _unitOfWork.CommitTransaction();

                return BaseResponseModel<string>.OkResponseModel(approvalUrl);
            }
            catch (CustomException)
            {
                _unitOfWork.RollBack();
                throw;
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw new CustomException(StatusCodes.Status500InternalServerError,
                    ResponseCodeConstants.INTERNAL_SERVER_ERROR,
                    ResponseMessages.INTERNAL_SERVER_ERROR);
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }

        public async Task<BaseResponseModel<SubscriptionResponseModel>> CompleteSubscription(string paymentId, string payerId, string token, CancellationToken cancellationToken = default)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                await _paypalClient.ExecutePayment(paymentId, payerId, token, cancellationToken); // Pass the cancellation token

                var subscription = await _unitOfWork.SubscriptionRepository.GetByPaymentIdAsync(paymentId);
                if (subscription == null)
                {
                    throw new CustomException(StatusCodes.Status404NotFound,
                        ResponseCodeConstants.NOT_FOUND);
                }

                subscription.PaymentStatus = "Completed";
                subscription.IsActive = true;
                subscription.LastUpdatedTime = DateTime.UtcNow;

                _unitOfWork.SubscriptionRepository.Update(subscription);
                await _unitOfWork.SaveAsync();

                var responseModel = new SubscriptionResponseModel(subscription);
                _unitOfWork.CommitTransaction();

                return BaseResponseModel<SubscriptionResponseModel>.OkResponseModel(responseModel);
            }
            catch (CustomException)
            {
                _unitOfWork.RollBack();
                throw;
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw new CustomException(StatusCodes.Status500InternalServerError,
                    ResponseCodeConstants.INTERNAL_SERVER_ERROR,
                    ResponseMessages.INTERNAL_SERVER_ERROR);
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }
    }
}
