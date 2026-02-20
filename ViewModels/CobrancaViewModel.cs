using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaCaixaPostalIA.ViewModels;

public class CobrancaViewModel
{
    public int Id { get; set; }
    public int IdCaixaPostal { get; set; }
    public short IdStatusCobranca { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataVencimento { get; set; }
    public DateTime? DataLiquidacao { get; set; }

    public string CodigoCaixa { get; set; } = string.Empty;
    public string NomeCliente { get; set; } = string.Empty;
    public string NomeStatus { get; set; } = string.Empty;

    public SelectList? ListaStatus { get; set; }
}
