using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaCaixaPostalIA.Data;

namespace SistemaCaixaPostalIA.Controllers;

public abstract class ControladorBase : Controller
{
    protected readonly AppDbContext Contexto;

    protected ControladorBase(AppDbContext contexto)
    {
        Contexto = contexto;
    }

    public override void OnActionExecuting(ActionExecutingContext contextoFiltro)
    {
        var socios = Contexto.Socios
            .OrderBy(socio => socio.Nome)
            .AsNoTracking()
            .ToList();

        ViewBag.ListaSocios = new SelectList(socios, "Id", "Nome", ObterIdSocioFiltro());
        ViewBag.IdSocioSelecionado = ObterIdSocioFiltro();

        base.OnActionExecuting(contextoFiltro);
    }

    protected short? ObterIdSocioFiltro()
    {
        if (!Request.Query.ContainsKey("socioId"))
            return null;

        if (!short.TryParse(Request.Query["socioId"], out var idSocio))
            return null;

        return idSocio;
    }
}
