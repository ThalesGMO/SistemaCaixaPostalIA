using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaCaixaPostalIA.ViewModels;

public class HistoricoPagamentoViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "A cobrança é obrigatória.")]
    public int IdCobranca { get; set; }

    [Required(ErrorMessage = "A forma de pagamento é obrigatória.")]
    public short IdFormaPagamento { get; set; }

    [Required(ErrorMessage = "A data de pagamento é obrigatória.")]
    [DataType(DataType.Date)]
    public DateTime DataPagamento { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "O valor pago é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor pago deve ser maior que zero.")]
    public decimal ValorPago { get; set; }

    [MaxLength(400, ErrorMessage = "A observação deve ter no máximo 400 caracteres.")]
    public string? Observacao { get; set; }

    public string? CodigoCaixa { get; set; }
    public string? NomeCliente { get; set; }
    public decimal ValorCobranca { get; set; }

    public SelectList? ListaFormasPagamento { get; set; }
}
