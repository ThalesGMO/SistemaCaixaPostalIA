namespace SistemaCaixaPostalIA.Models;

public class CaixaPostal
{
    public int Id { get; set; }
    public short IdSocio { get; set; }
    public int IdCliente { get; set; }
    public int IdStatusCaixa { get; set; }
    public int IdTipoCaixa { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string? NomeEmpresa { get; set; }
    public string CpfCnpj { get; set; } = string.Empty;
    public DateTime? DataAluguel { get; set; }
    public int DiaVencimento { get; set; }
    public decimal Valor { get; set; }

    public virtual Socio? Socio { get; set; }
    public virtual Cliente? Cliente { get; set; }
    public virtual CaixaStatus? CaixaStatus { get; set; }
    public virtual TipoCaixa? TipoCaixa { get; set; }
    public virtual ICollection<Cobranca> Cobrancas { get; set; } = [];
}
