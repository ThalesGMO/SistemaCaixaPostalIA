using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaCaixaPostalIA.ViewModels;

public class ClienteViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [MaxLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(320, ErrorMessage = "O email deve ter no máximo 320 caracteres.")]
    [EmailAddress(ErrorMessage = "Informe um email válido.")]
    public string? Email { get; set; }

    [MaxLength(15, ErrorMessage = "O telefone deve ter no máximo 15 caracteres.")]
    public string? Telefone { get; set; }

    public int IdClienteStatus { get; set; }

    public string? NomeStatus { get; set; }

    public SelectList? ListaStatus { get; set; }
}
