using System;

namespace FacarPDV.Models
{
    public class VendaListItemVM
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = "";   // ex.: V-0001
        public string Cliente { get; set; } = "";
        public DateTime DataHora { get; set; }
        public decimal Valor { get; set; }

        public int? UsuarioId { get; set; }        
        public string Usuario { get; set; } = "";  
    }
}
