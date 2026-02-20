namespace SistemaCaixaPostalIA.Models;

public class Socio
{
    public short Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Numero { get; set; }

    public virtual ICollection<CaixaPostal> CaixasPostais { get; set; } = new List<CaixaPostal>();
}
