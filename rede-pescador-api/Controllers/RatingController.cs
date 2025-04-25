using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using rede_pescador_api.Dto;
using rede_pescador_api.Services;
using Microsoft.AspNetCore.Authorization;

namespace rede_pescador_api.Controllers
{
    [ApiController]
    [Route("rede-pescador/avaliações")]
    public class RatingController : ControllerBase
    {
        private readonly RatingService _ratingService;

        public RatingController(RatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [Authorize(Roles = "CONSUMIDOR")]
        [HttpPost("avaliar")]
        public async Task<IActionResult> AddRating([FromBody] RatingDTO ratingDTO)
        {
            try
            {
                await _ratingService.AddRatingAsync(ratingDTO.OrderId, ratingDTO.Score, ratingDTO.Comment);
                return Ok(new { message = "Avaliação registrada com sucesso." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize("PESCADOR")]
        [HttpGet("pedido/{orderId}")]
        public async Task<IActionResult> GetRatingByOrder(long orderId)
        {
            var rating = await _ratingService.GetRatingByOrderIdAsync(orderId);
            if (rating == null)
                return NotFound();

            return Ok(rating);
        }
    }
}
