using rede_pescador_api.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("orders")]
public class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public long Id { get; set; }

    [Column("buyer_id")]
    public long BuyerId { get; set; }  //FK de users

    [Column("seller_id")]
    public long SellerId { get; set; }  //FK de users

    [Column("product_id")]
    public long ProductId { get; set; }  //FK de produtos

    [Column("delivery_method")]
    public DeliveryMethod DeliveryMethod { get; set; }

    [Column("status")]
    public OrderStatus Status { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}
