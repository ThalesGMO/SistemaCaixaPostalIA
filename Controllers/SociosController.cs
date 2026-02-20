using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaCaixaPostalIA.Data;
using SistemaCaixaPostalIA.Models;
using SistemaCaixaPostalIA.ViewModels;

namespace SistemaCaixaPostalIA.Controllers;

public class SociosController : ControladorBase
{
    public SociosController(AppDbContext contexto) : base(contexto) { }

    public async Task<IActionResult> Index()
    {
        var socios = await Contexto.Socios
            .AsNoTracking()
            .OrderBy(socio => socio.Nome)
            .ToListAsync();

        return View(socios);
    }

    public IActionResult Criar()
    {
        return View(new SocioViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Criar(SocioViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        var socio = new Socio
        {
            Nome = viewModel.Nome,
            Email = viewModel.Email,
            Numero = viewModel.Numero
        };

        Contexto.Socios.Add(socio);
        await Contexto.SaveChangesAsync();

        TempData["Sucesso"] = "Sócio cadastrado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Editar(short id)
    {
        var socio = await Contexto.Socios.FindAsync(id);

        if (socio is null)
            return NotFound();

        var viewModel = new SocioViewModel
        {
            Id = socio.Id,
            Nome = socio.Nome,
            Email = socio.Email,
            Numero = socio.Numero
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(short id, SocioViewModel viewModel)
    {
        if (id != viewModel.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(viewModel);

        var socio = await Contexto.Socios.FindAsync(id);

        if (socio is null)
            return NotFound();

        socio.Nome = viewModel.Nome;
        socio.Email = viewModel.Email;
        socio.Numero = viewModel.Numero;

        await Contexto.SaveChangesAsync();

        TempData["Sucesso"] = "Sócio atualizado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Excluir(short id)
    {
        var socio = await Contexto.Socios.FindAsync(id);

        if (socio is null)
            return NotFound();

        return View(socio);
    }

    [HttpPost, ActionName("Excluir")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmarExclusao(short id)
    {
        var socio = await Contexto.Socios.FindAsync(id);

        if (socio is null)
            return NotFound();

        var possuiCaixasVinculadas = await Contexto.CaixasPostais
            .AnyAsync(caixa => caixa.IdSocio == id);

        if (possuiCaixasVinculadas)
        {
            TempData["Erro"] = "Não é possível excluir o sócio pois existem caixas postais vinculadas.";
            return RedirectToAction(nameof(Index));
        }

        Contexto.Socios.Remove(socio);
        await Contexto.SaveChangesAsync();

        TempData["Sucesso"] = "Sócio excluído com sucesso!";
        return RedirectToAction(nameof(Index));
    }
}
