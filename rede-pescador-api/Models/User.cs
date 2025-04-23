using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rede_pescador_api.Models
{
 

    [Table("users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("name")]
        public string Name { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        [Column("email")]
        public string Email { get; set; }

        [Phone]
        [MaxLength(20)]
        [Column("phone")]
        public string Phone { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("password")]
        public string PasswordHash { get; set; }

        [Required]
        [Column("role")]
        public string Role { get; set; } // PESCADOR/CONSUMIDOR/ESTABELECIMENTO

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
