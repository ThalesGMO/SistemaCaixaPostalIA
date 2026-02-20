namespace SistemaCaixaPostalIA.Models;

public class Cliente
{
    public int Id { get; set; }
    public int IdClienteStatus { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Telefone { get; set; }

    public virtual ClienteStatus? ClienteStatus { get; set; }
    public virtual ICollection<CaixaPostal> CaixasPostais { get; set; } = new List<CaixaPostal>();
}
