using System.ComponentModel.DataAnnotations;

namespace SistemaCaixaPostalIA.ViewModels;

public class SocioViewModel
{
    public short Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [MaxLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O email é obrigatório.")]
    [MaxLength(320, ErrorMessage = "O email deve ter no máximo 320 caracteres.")]
    [EmailAddress(ErrorMessage = "Informe um email válido.")]
    public string Email { get; set; } = string.Empty;

    [MaxLength(11, ErrorMessage = "O número deve ter no máximo 11 caracteres.")]
    public string? Numero { get; set; }
}
