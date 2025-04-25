namespace rede_pescador_api.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    namespace rede_pescador_api.Models
    {
        [Table("ratings")]
        public class Rating
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            [Column("id")]
            public long Id { get; set; }
            [Required]
            [Column("order_id")]
            public long OrderId { get; set; } // FK para Order
            [Required]
            [ForeignKey("OrderId")]
            public Order Order { get; set; }  // Propriedade de navegação
            [Required]
            [Column("score")]
            public int Score { get; set; } // Avaliação (estrelas)
  
            [Column("comment")]
            public string? Comment { get; set; }
            [Required]
            [Column("rating_date")]
            public DateTime RatingDate { get; set; }
        }
    }

}
