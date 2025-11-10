namespace FacarPDV.Models
{
    public class VendaListItemVM
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = "";   // ex.: V-0001
        public string Cliente { get; set; } = "";
        public DateTime DataHora { get; set; }
        public string? Pagamento { get; set; }      // Dinheiro/Cartão/Pix
        public string Status { get; set; } = "Pendente"; // Pago/Pendente/Cancelado
        public decimal Valor { get; set; }
    }
}
