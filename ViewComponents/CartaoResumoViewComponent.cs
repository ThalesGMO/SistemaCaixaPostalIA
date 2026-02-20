using Microsoft.AspNetCore.Mvc;

namespace SistemaCaixaPostalIA.ViewComponents;

public class CartaoResumoViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string titulo, string valor, string icone, string classCor)
    {
        ViewBag.Titulo = titulo;
        ViewBag.Valor = valor;
        ViewBag.Icone = icone;
        ViewBag.ClasseCor = classCor;
        return View();
    }
}
