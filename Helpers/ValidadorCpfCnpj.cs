namespace SistemaCaixaPostalIA.Helpers;

public static class ValidadorCpfCnpj
{
    public static bool PossuiTamanhoValido(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            return false;

        var apenasDigitos = new string(valor.Where(char.IsDigit).ToArray());
        return apenasDigitos.Length == 11 || apenasDigitos.Length == 14;
    }
}
