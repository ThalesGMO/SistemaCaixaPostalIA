namespace SistemaCaixaPostalIA.ViewModels;

public class DashboardViewModel
{
    public decimal TotalRecebido { get; set; }
    public decimal TotalPendente { get; set; }
    public decimal TotalAtrasado { get; set; }
    public int QuantidadeCaixasAtivas { get; set; }

    public int CobrancasPendentes { get; set; }
    public int CobrancasPagas { get; set; }
    public int CobrancasAtrasadas { get; set; }

    public List<string> RotulosMeses { get; set; } = new();
    public List<decimal> ValoresPorMes { get; set; } = new();

    public List<CobrancaAlertaViewModel> ProximosVencimentos { get; set; } = new();
    public List<CobrancaAlertaViewModel> CobrancasEmAtraso { get; set; } = new();
}

public class CobrancaAlertaViewModel
{
    public string CodigoCaixa { get; set; } = string.Empty;
    public string NomeCliente { get; set; } = string.Empty;
    public DateTime DataVencimento { get; set; }
    public decimal Valor { get; set; }
    public string NomeStatus { get; set; } = string.Empty;
}
