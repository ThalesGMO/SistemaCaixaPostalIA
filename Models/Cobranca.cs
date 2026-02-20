namespace SistemaCaixaPostalIA.Models;

public class Cobranca
{
    public int Id { get; set; }
    public int IdCaixaPostal { get; set; }
    public short IdStatusCobranca { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataVencimento { get; set; }
    public DateTime? DataLiquidacao { get; set; }

    public virtual CaixaPostal? CaixaPostal { get; set; }
    public virtual CobrancaStatus? CobrancaStatus { get; set; }
    public virtual ICollection<HistoricoPagamento> HistoricoPagamentos { get; set; } = new List<HistoricoPagamento>();
}
