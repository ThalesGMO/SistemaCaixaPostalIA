using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaCaixaPostalIA.Data;
using SistemaCaixaPostalIA.Helpers;
using SistemaCaixaPostalIA.Models;
using SistemaCaixaPostalIA.Models.Enums;
using SistemaCaixaPostalIA.ViewModels;

namespace SistemaCaixaPostalIA.Controllers;

public class CaixasPostaisController : ControladorBase
{
    public CaixasPostaisController(AppDbContext contexto) : base(contexto) { }

    public async Task<IActionResult> Index()
    {
        var idSocio = ObterIdSocioFiltro();

        var consulta = Contexto.CaixasPostais
            .Include(caixa => caixa.Cliente)
            .Include(caixa => caixa.Socio)
            .Include(caixa => caixa.CaixaStatus)
            .Include(caixa => caixa.TipoCaixa)
            .AsNoTracking();

        if (idSocio.HasValue)
            consulta = consulta.Where(caixa => caixa.IdSocio == idSocio.Value);

        var caixas = await consulta
            .OrderBy(caixa => caixa.Codigo)
            .ToListAsync();

        return View(caixas);
    }

    public async Task<IActionResult> Detalhes(int id)
    {
        var caixa = await Contexto.CaixasPostais
            .Include(caixa => caixa.Cliente)
            .Include(caixa => caixa.Socio)
            .Include(caixa => caixa.CaixaStatus)
            .Include(caixa => caixa.TipoCaixa)
            .Include(caixa => caixa.Cobrancas)
                .ThenInclude(cobranca => cobranca.CobrancaStatus)
            .AsNoTracking()
            .FirstOrDefaultAsync(caixa => caixa.Id == id);

        if (caixa is null)
            return NotFound();

        return View(caixa);
    }

    public async Task<IActionResult> Criar()
    {
        var viewModel = new CaixaPostalViewModel
        {
            IdStatusCaixa = (int)CaixaStatusEnum.Ativa,
            IdTipoCaixa = (int)TipoCaixaEnum.Mensalidade,
            DataAluguel = DateTime.Today
        };

        await PreencherListasDropdown(viewModel);
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Criar(CaixaPostalViewModel viewModel)
    {
        if (!ValidarCpfCnpj(viewModel.CpfCnpj))
            ModelState.AddModelError("CpfCnpj", "O CPF deve ter 11 dígitos ou o CNPJ deve ter 14 dígitos.");

        ValidarPorTipo(viewModel);

        if (!ModelState.IsValid)
        {
            await PreencherListasDropdown(viewModel);
            return View(viewModel);
        }

        if (viewModel.IdTipoCaixa == (int)TipoCaixaEnum.Cortesia)
            viewModel.Valor = 0;

        var caixa = new CaixaPostal
        {
            IdSocio = viewModel.IdSocio,
            IdCliente = viewModel.IdCliente,
            IdStatusCaixa = viewModel.IdStatusCaixa,
            IdTipoCaixa = viewModel.IdTipoCaixa,
            Codigo = viewModel.Codigo,
            NomeEmpresa = viewModel.NomeEmpresa,
            CpfCnpj = viewModel.CpfCnpj,
            DataAluguel = viewModel.DataAluguel,
            DiaVencimento = viewModel.DiaVencimento,
            Valor = viewModel.Valor
        };

        Contexto.CaixasPostais.Add(caixa);
        await Contexto.SaveChangesAsync();

        TempData["Sucesso"] = "Caixa postal cadastrada com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Editar(int id)
    {
        var caixa = await Contexto.CaixasPostais.FindAsync(id);

        if (caixa is null)
            return NotFound();

        var viewModel = new CaixaPostalViewModel
        {
            Id = caixa.Id,
            IdSocio = caixa.IdSocio,
            IdCliente = caixa.IdCliente,
            IdStatusCaixa = caixa.IdStatusCaixa,
            IdTipoCaixa = caixa.IdTipoCaixa,
            Codigo = caixa.Codigo,
            NomeEmpresa = caixa.NomeEmpresa,
            CpfCnpj = caixa.CpfCnpj,
            DataAluguel = caixa.DataAluguel,
            DiaVencimento = caixa.DiaVencimento,
            Valor = caixa.Valor
        };

        await PreencherListasDropdown(viewModel);
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, CaixaPostalViewModel viewModel)
    {
        if (id != viewModel.Id)
            return BadRequest();

        if (!ValidarCpfCnpj(viewModel.CpfCnpj))
            ModelState.AddModelError("CpfCnpj", "O CPF deve ter 11 dígitos ou o CNPJ deve ter 14 dígitos.");

        ValidarPorTipo(viewModel);

        if (!ModelState.IsValid)
        {
            await PreencherListasDropdown(viewModel);
            return View(viewModel);
        }

        if (viewModel.IdTipoCaixa == (int)TipoCaixaEnum.Cortesia)
            viewModel.Valor = 0;

        var caixa = await Contexto.CaixasPostais.FindAsync(id);

        if (caixa is null)
            return NotFound();

        caixa.IdSocio = viewModel.IdSocio;
        caixa.IdCliente = viewModel.IdCliente;
        caixa.IdStatusCaixa = viewModel.IdStatusCaixa;
        caixa.IdTipoCaixa = viewModel.IdTipoCaixa;
        caixa.Codigo = viewModel.Codigo;
        caixa.NomeEmpresa = viewModel.NomeEmpresa;
        caixa.CpfCnpj = viewModel.CpfCnpj;
        caixa.DataAluguel = viewModel.DataAluguel;
        caixa.DiaVencimento = viewModel.DiaVencimento;
        caixa.Valor = viewModel.Valor;

        await Contexto.SaveChangesAsync();

        TempData["Sucesso"] = "Caixa postal atualizada com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Excluir(int id)
    {
        var caixa = await Contexto.CaixasPostais
            .Include(caixa => caixa.Cliente)
            .Include(caixa => caixa.Socio)
            .Include(caixa => caixa.CaixaStatus)
            .Include(caixa => caixa.TipoCaixa)
            .AsNoTracking()
            .FirstOrDefaultAsync(caixa => caixa.Id == id);

        if (caixa is null)
            return NotFound();

        return View(caixa);
    }

    [HttpPost, ActionName("Excluir")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmarExclusao(int id)
    {
        var caixa = await Contexto.CaixasPostais.FindAsync(id);

        if (caixa is null)
            return NotFound();

        var possuiCobrancas = await Contexto.Cobrancas
            .AnyAsync(cobranca => cobranca.IdCaixaPostal == id);

        if (possuiCobrancas)
        {
            TempData["Erro"] = "Não é possível excluir a caixa postal pois existem cobranças vinculadas.";
            return RedirectToAction(nameof(Index));
        }

        Contexto.CaixasPostais.Remove(caixa);
        await Contexto.SaveChangesAsync();

        TempData["Sucesso"] = "Caixa postal excluída com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    private bool ValidarCpfCnpj(string cpfCnpj)
    {
        return ValidadorCpfCnpj.PossuiTamanhoValido(cpfCnpj);
    }

    private void ValidarPorTipo(CaixaPostalViewModel viewModel)
    {
        if (viewModel.IdTipoCaixa != (int)TipoCaixaEnum.Cortesia && viewModel.Valor <= 0)
            ModelState.AddModelError("Valor", "O valor deve ser maior que zero.");
    }

    private async Task PreencherListasDropdown(CaixaPostalViewModel viewModel)
    {
        var socios = await Contexto.Socios.AsNoTracking().OrderBy(socio => socio.Nome).ToListAsync();
        var clientes = await Contexto.Clientes.AsNoTracking().OrderBy(cliente => cliente.Nome).ToListAsync();
        var statusList = await Contexto.CaixasStatus.AsNoTracking().OrderBy(status => status.Id).ToListAsync();
        var tiposList = await Contexto.TiposCaixa.AsNoTracking().OrderBy(tipo => tipo.Id).ToListAsync();

        viewModel.ListaSocios = new SelectList(socios, "Id", "Nome", viewModel.IdSocio);
        viewModel.ListaClientes = new SelectList(clientes, "Id", "Nome", viewModel.IdCliente);
        viewModel.ListaStatus = new SelectList(statusList, "Id", "Nome", viewModel.IdStatusCaixa);
        viewModel.ListaTipos = new SelectList(tiposList, "Id", "Nome", viewModel.IdTipoCaixa);
    }
}
