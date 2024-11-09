using CursusJapaneseLearningPlatform.Service.BusinessModels.FlashcardModels;
using CursusJapaneseLearningPlatform.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CursusJapaneseLearningPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FlashcardsController : ControllerBase
    {
        private readonly IFlashcardService _flashcardService;

        public FlashcardsController(IFlashcardService flashcardService)
        {
            _flashcardService = flashcardService;
        }

        [HttpPost]
        [Authorize(Roles = "Learner")]
        public async Task<IActionResult> Create([FromBody] CreateFlashcardDto dto)
        {
            var response = await _flashcardService.CreateAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Learner")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _flashcardService.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Learner")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _flashcardService.GetByIdAsync(id);
            return response.StatusCode == StatusCodes.Status200OK
                ? Ok(response)
                : StatusCode(response.StatusCode, response);
        }

        [HttpGet("collection/{collectionId}")]
        [Authorize(Roles = "Learner")]
        public async Task<IActionResult> GetAllByCollection(Guid collectionId)
        {
            var response = await _flashcardService.GetAllByCollectionIdAsync(collectionId);
            return response.StatusCode == StatusCodes.Status200OK
                ? Ok(response)
                : StatusCode(response.StatusCode, response);
        }
    }
}
