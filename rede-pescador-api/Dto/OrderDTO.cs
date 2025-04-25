using rede_pescador_api.Models.Enums;

namespace rede_pescador_api.Dto
{
    public class OrderDTO
    {
        public long Id { get; set; }
        public long BuyerId { get; set; }
        public long ProductId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
