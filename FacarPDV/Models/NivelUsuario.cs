using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacarPDV.Models
{
    [Table("NivelUsuario")]
    public class NivelUsuario
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("NivelID_PK")] 
        public int NivelId { get; set; }

        [Required]
        [Column("Descricao")]
        public string Descricao { get; set; } = string.Empty;
    }
}
