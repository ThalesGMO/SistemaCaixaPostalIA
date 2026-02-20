namespace SistemaCaixaPostalIA.Models;

public class HistoricoPagamento
{
    public int Id { get; set; }
    public int IdCobranca { get; set; }
    public short IdFormaPagamento { get; set; }
    public DateTime DataPagamento { get; set; }
    public decimal ValorPago { get; set; }
    public string? Observacao { get; set; }

    public virtual Cobranca? Cobranca { get; set; }
    public virtual FormaPagamento? FormaPagamento { get; set; }
}
