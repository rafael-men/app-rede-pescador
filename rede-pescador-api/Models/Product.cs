using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using rede_pescador_api.Models.Enums;

namespace rede_pescador_api.Models
{
    [Table("products")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }
        [Required]
        [Column("tipo")]
        public FishTypes Tipo  { get; set; }

        [Required]
        [Column("nome")]
        public string Nome { get; set; }

        [Required]
        [Column("localizacao")]
        public string Localizacao  { get; set; }

        [Required]
        [Column("descricao")]
        public string Descricao { get; set; }

        [Required]
        [Range(0.1, double.MaxValue)]
        [Column("peso_kg")]
        public decimal PesoKg { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        [Column("preco_quilo")]
        public decimal PrecoQuilo { get; set; }
        [Required]
        [Column("imagem_url")]
        public string? ImagemURL { get; set; }
        [Column("avaliavel")]
        public bool Avaliável { get; set; }

        [Required]
        [Column("id_pescador")]
        public long IdPescador { get; set; }
    }
}
