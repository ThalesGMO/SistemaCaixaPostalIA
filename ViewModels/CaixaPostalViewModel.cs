using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaCaixaPostalIA.ViewModels;

public class CaixaPostalViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O sócio é obrigatório.")]
    public short IdSocio { get; set; }

    [Required(ErrorMessage = "O cliente é obrigatório.")]
    public int IdCliente { get; set; }

    public int IdStatusCaixa { get; set; }

    [Required(ErrorMessage = "O código da caixa é obrigatório.")]
    [MaxLength(100, ErrorMessage = "O código deve ter no máximo 100 caracteres.")]
    public string Codigo { get; set; } = string.Empty;

    [MaxLength(200, ErrorMessage = "O nome da empresa deve ter no máximo 200 caracteres.")]
    public string? NomeEmpresa { get; set; }

    [Required(ErrorMessage = "O CPF/CNPJ é obrigatório.")]
    [MaxLength(14, ErrorMessage = "O CPF/CNPJ deve ter no máximo 14 caracteres.")]
    public string CpfCnpj { get; set; } = string.Empty;

    [Required(ErrorMessage = "A data de aluguel é obrigatória.")]
    [DataType(DataType.Date)]
    public DateTime DataAluguel { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "O dia de vencimento é obrigatório.")]
    [Range(1, 31, ErrorMessage = "O dia de vencimento deve ser entre 1 e 31.")]
    public int DiaVencimento { get; set; }

    [Required(ErrorMessage = "O valor mensal é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor mensal deve ser maior que zero.")]
    public decimal ValorMensal { get; set; }

    public string? NomeSocio { get; set; }
    public string? NomeCliente { get; set; }
    public string? NomeStatus { get; set; }

    public SelectList? ListaSocios { get; set; }
    public SelectList? ListaClientes { get; set; }
    public SelectList? ListaStatus { get; set; }
}
