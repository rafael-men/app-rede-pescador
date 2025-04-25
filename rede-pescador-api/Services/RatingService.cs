using rede_pescador_api.Dto;
using rede_pescador_api.Models.Enums;
using rede_pescador_api.Models.rede_pescador_api.Models;
using rede_pescador_api.Orders;

namespace rede_pescador_api.Services
{
    public class RatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IOrderRepository _orderRepository;

        public RatingService(IRatingRepository ratingRepository, IOrderRepository orderRepository)
        {
            _ratingRepository = ratingRepository;
            _orderRepository = orderRepository;
        }

        public async Task<RatingDTO> AddRatingAsync(long orderId, int score, string comment)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new Exception("Pedido não encontrado.");
            if (order == null || order.Status != OrderStatus.Entregue)
                throw new Exception("A avaliação só pode ser feita após a entrega.");
            if(score > 5 && score < 0)
            {
                throw new Exception("Selecione um valor entre 1 e 5 estrelas.");
            }

            var rating = new Rating
            {
                OrderId = orderId,
                Score = score,
                Comment = comment,
                RatingDate = DateTime.UtcNow
            };

            await _ratingRepository.AddAsync(rating);

            return new RatingDTO
            {
                OrderId = rating.OrderId,
                Score = rating.Score,
                Comment = rating.Comment,
                RatingDate = rating.RatingDate
            };
        }

        public async Task<RatingDTO?> GetRatingByOrderIdAsync(long orderId)
        {
            var rating = await _ratingRepository.GetByOrderIdAsync(orderId);
            if (rating == null) return null;

            return new RatingDTO
            {
                Id = rating.Id,
                OrderId = rating.OrderId,
                Score = rating.Score,
                Comment = rating.Comment,
                RatingDate = rating.RatingDate
            };
        }
    }
}
