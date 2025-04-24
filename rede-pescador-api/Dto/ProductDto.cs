using System.ComponentModel.DataAnnotations;
using rede_pescador_api.Models.Enums;

namespace rede_pescador_api.Dto
{
    public class ProductDTO
    {
        public long Id { get; set; }
        public FishTypes Tipo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal PesoKg { get; set; }

        public string Localizacao { get; set; }
        public decimal PrecoQuilo { get; set; }
        public string ImagemURL{ get; set; }
        public bool Avaliável { get; set; }
        public long IdPescador { get; set; }
    }
}
