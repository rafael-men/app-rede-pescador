using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MercadoPago.Resource.Payment;
using rede_pescador_api.Models.Enums.rede_pescador_api.Models.Enums;

namespace rede_pescador_api.Models
{
    [Table("payment")]
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }
        [Required]
        [Column("transaction_id")]
        public string TransactionId { get; set; } // ID do gateway 
        [Required]
        [Column("amount")]
        public decimal Amount { get; set; }
        [Required]
        [Column("method")]
        public string Method { get; set; } // PIX
        [Required]
        [Column("status")]
        public Enums.rede_pescador_api.Models.Enums.PaymentStatus Status { get; set; } // PENDING, PAID, FAILED
        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        [Column("order_id")]
        public long OrderId { get; set; }
        public Order Order { get; set; }
    }

}
