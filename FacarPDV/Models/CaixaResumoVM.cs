namespace FacarPDV.Models
{
    public class CaixaResumoVM
    {
        public DateTime Data { get; set; }

        // Esperado (somado das vendas "Pagas" do dia)
        public decimal EsperadoDinheiro { get; set; }
        public decimal EsperadoCartao { get; set; }
        public decimal EsperadoPix { get; set; }
        public decimal EsperadoTotal => EsperadoDinheiro + EsperadoCartao + EsperadoPix;

        // Contado (informado no POST)
        public decimal ContadoDinheiro { get; set; }
        public decimal ContadoCartao { get; set; }
        public decimal ContadoPix { get; set; }
        public decimal ContadoTotal => ContadoDinheiro + ContadoCartao + ContadoPix;

        // Diferenças
        public decimal DifDinheiro => ContadoDinheiro - EsperadoDinheiro;
        public decimal DifCartao => ContadoCartao - EsperadoCartao;
        public decimal DifPix => ContadoPix - EsperadoPix;
        public decimal DifTotal => ContadoTotal - EsperadoTotal;

        // Lista de vendas do dia
        public List<VendaLinhaVM> VendasDia { get; set; } = new();
        public string? Observacao { get; set; }
    }

    public class VendaLinhaVM
    {
        public int Id { get; set; }
        public string Cliente { get; set; } = "";
        public DateTime DataHora { get; set; }
        public string FormaPagamento { get; set; } = ""; // ex.: "Dinheiro" | "Cartão" | "Pix"
        public string Status { get; set; } = "";         // ex.: "Pago" | "Pendente" | "Cancelado"
        public decimal Valor { get; set; }
    }
}
