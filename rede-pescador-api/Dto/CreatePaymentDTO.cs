namespace rede_pescador_api.Dto
{
    public class CreatePaymentDTO
    {
        public long OrderId { get; set; }
        public decimal Amount { get; set; }
    }

}
