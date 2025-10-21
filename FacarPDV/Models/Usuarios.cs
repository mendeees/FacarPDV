using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacarPDV.Models;

[Table("Usuarios")]
public class Usuarios
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ID_PK")]
    public int Id { get; set; }
    [Required]
    [Column("Nome")]
    public string Nome { get; set; }
    [Required]
    [Column("Login")]
    public string Login { get; set; }
    [Required]
    [Column("Senha")]
    public string Senha { get; set; }
    [Required]
    [Column("NivelID_FK")]
    public int NivelId { get; set; }
}
