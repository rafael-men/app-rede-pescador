namespace rede_pescador_api.Dto
{
    public class RatingDTO
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public int Score { get; set; }
        public string? Comment { get; set; }
        public DateTime RatingDate { get; set; }
    }

}
