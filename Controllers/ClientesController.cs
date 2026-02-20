using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaCaixaPostalIA.Data;
using SistemaCaixaPostalIA.Models;
using SistemaCaixaPostalIA.Models.Enums;
using SistemaCaixaPostalIA.ViewModels;

namespace SistemaCaixaPostalIA.Controllers;

public class ClientesController : ControladorBase
{
    public ClientesController(AppDbContext contexto) : base(contexto) { }

    public async Task<IActionResult> Index(string? termoBusca)
    {
        await AtualizarClientesInadimplentes();

        var consulta = Contexto.Clientes
            .Include(cliente => cliente.ClienteStatus)
            .AsNoTracking();

        var idSocio = ObterIdSocioFiltro();
        if (idSocio.HasValue)
            consulta = consulta.Where(cliente =>
                cliente.CaixasPostais.Any(caixa => caixa.IdSocio == idSocio.Value));

        if (!string.IsNullOrWhiteSpace(termoBusca))
            consulta = consulta.Where(cliente =>
                cliente.Nome.Contains(termoBusca));

        ViewBag.TermoBusca = termoBusca;

        var clientes = await consulta
            .OrderBy(cliente => cliente.Nome)
            .ToListAsync();

        return View(clientes);
    }

    public async Task<IActionResult> Detalhes(int id)
    {
        var cliente = await Contexto.Clientes
            .Include(cliente => cliente.ClienteStatus)
            .Include(cliente => cliente.CaixasPostais)
                .ThenInclude(caixa => caixa.CaixaStatus)
            .Include(cliente => cliente.CaixasPostais)
                .ThenInclude(caixa => caixa.Socio)
            .AsNoTracking()
            .FirstOrDefaultAsync(cliente => cliente.Id == id);

        if (cliente is null)
            return NotFound();

        return View(cliente);
    }

    public async Task<IActionResult> Criar()
    {
        var viewModel = new ClienteViewModel
        {
            IdClienteStatus = (int)ClienteStatusEnum.Ativo,
            ListaStatus = await ObterListaStatus()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Criar(ClienteViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            viewModel.ListaStatus = await ObterListaStatus();
            return View(viewModel);
        }

        var cliente = new Cliente
        {
            Nome = viewModel.Nome,
            Email = viewModel.Email,
            Telefone = viewModel.Telefone,
            IdClienteStatus = viewModel.IdClienteStatus
        };

        Contexto.Clientes.Add(cliente);
        await Contexto.SaveChangesAsync();

        TempData["Sucesso"] = "Cliente cadastrado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Editar(int id)
    {
        var cliente = await Contexto.Clientes.FindAsync(id);

        if (cliente is null)
            return NotFound();

        var viewModel = new ClienteViewModel
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Email = cliente.Email,
            Telefone = cliente.Telefone,
            IdClienteStatus = cliente.IdClienteStatus,
            ListaStatus = await ObterListaStatus()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, ClienteViewModel viewModel)
    {
        if (id != viewModel.Id)
            return BadRequest();

        if (!ModelState.IsValid)
        {
            viewModel.ListaStatus = await ObterListaStatus();
            return View(viewModel);
        }

        var cliente = await Contexto.Clientes.FindAsync(id);

        if (cliente is null)
            return NotFound();

        cliente.Nome = viewModel.Nome;
        cliente.Email = viewModel.Email;
        cliente.Telefone = viewModel.Telefone;
        cliente.IdClienteStatus = viewModel.IdClienteStatus;

        await Contexto.SaveChangesAsync();

        TempData["Sucesso"] = "Cliente atualizado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Excluir(int id)
    {
        var cliente = await Contexto.Clientes
            .Include(cliente => cliente.ClienteStatus)
            .AsNoTracking()
            .FirstOrDefaultAsync(cliente => cliente.Id == id);

        if (cliente is null)
            return NotFound();

        return View(cliente);
    }

    [HttpPost, ActionName("Excluir")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmarExclusao(int id)
    {
        var cliente = await Contexto.Clientes.FindAsync(id);

        if (cliente is null)
            return NotFound();

        var possuiCaixasVinculadas = await Contexto.CaixasPostais
            .AnyAsync(caixa => caixa.IdCliente == id);

        if (possuiCaixasVinculadas)
        {
            TempData["Erro"] = "Não é possível excluir o cliente pois existem caixas postais vinculadas.";
            return RedirectToAction(nameof(Index));
        }

        Contexto.Clientes.Remove(cliente);
        await Contexto.SaveChangesAsync();

        TempData["Sucesso"] = "Cliente excluído com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    private async Task AtualizarClientesInadimplentes()
    {
        var hoje = DateTime.Today;
        var limiteAtraso = hoje.AddDays(-5);

        var clientesParaMarcar = await Contexto.Clientes
            .Where(cliente => cliente.IdClienteStatus != (int)ClienteStatusEnum.Inadimplente)
            .Where(cliente => cliente.CaixasPostais.Any(caixa =>
                caixa.Cobrancas.Any(cobranca =>
                    cobranca.IdStatusCobranca != (short)CobrancaStatusEnum.Pago
                    && cobranca.IdStatusCobranca != (short)CobrancaStatusEnum.Cancelado
                    && cobranca.DataVencimento < limiteAtraso)))
            .ToListAsync();

        foreach (var cliente in clientesParaMarcar)
            cliente.IdClienteStatus = (int)ClienteStatusEnum.Inadimplente;

        if (clientesParaMarcar.Count > 0)
            await Contexto.SaveChangesAsync();
    }

    private async Task<SelectList> ObterListaStatus()
    {
        var listaStatus = await Contexto.ClientesStatus
            .AsNoTracking()
            .OrderBy(status => status.Id)
            .ToListAsync();

        return new SelectList(listaStatus, "Id", "Nome");
    }
}
