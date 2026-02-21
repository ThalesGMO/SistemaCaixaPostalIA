using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaCaixaPostalIA.Data;
using SistemaCaixaPostalIA.Models;
using SistemaCaixaPostalIA.Models.Enums;
using SistemaCaixaPostalIA.ViewModels;

namespace SistemaCaixaPostalIA.Controllers;

public class CobrancasController : ControladorBase
{
    public CobrancasController(AppDbContext contexto) : base(contexto) { }

    public async Task<IActionResult> Index(short? statusId)
    {
        await AtualizarCobrancasAtrasadas();

        var idSocio = ObterIdSocioFiltro();

        var consulta = Contexto.Cobrancas
            .Include(cobranca => cobranca.CaixaPostal)
                .ThenInclude(caixa => caixa!.Cliente)
            .Include(cobranca => cobranca.CaixaPostal)
                .ThenInclude(caixa => caixa!.Socio)
            .Include(cobranca => cobranca.CobrancaStatus)
            .AsNoTracking();

        if (idSocio.HasValue)
            consulta = consulta.Where(cobranca => cobranca.CaixaPostal!.IdSocio == idSocio.Value);

        if (statusId.HasValue)
            consulta = consulta.Where(cobranca => cobranca.IdStatusCobranca == statusId.Value);

        var cobrancas = await consulta
            .OrderByDescending(cobranca => cobranca.DataVencimento)
            .Select(cobranca => new CobrancaViewModel
            {
                Id = cobranca.Id,
                IdCaixaPostal = cobranca.IdCaixaPostal,
                IdStatusCobranca = cobranca.IdStatusCobranca,
                Valor = cobranca.Valor,
                DataVencimento = cobranca.DataVencimento,
                DataLiquidacao = cobranca.DataLiquidacao,
                CodigoCaixa = cobranca.CaixaPostal!.Codigo,
                NomeCliente = cobranca.CaixaPostal.Cliente!.Nome,
                NomeStatus = cobranca.CobrancaStatus!.Nome
            })
            .ToListAsync();

        var listaStatus = await Contexto.CobrancasStatus.AsNoTracking().OrderBy(status => status.Id).ToListAsync();
        ViewBag.ListaStatusCobranca = new SelectList(listaStatus, "Id", "Nome", statusId);
        ViewBag.StatusIdSelecionado = statusId;

        return View(cobrancas);
    }

    public async Task<IActionResult> Detalhes(int id)
    {
        var cobranca = await Contexto.Cobrancas
            .Include(cobranca => cobranca.CaixaPostal)
                .ThenInclude(caixa => caixa!.Cliente)
            .Include(cobranca => cobranca.CaixaPostal)
                .ThenInclude(caixa => caixa!.Socio)
            .Include(cobranca => cobranca.CobrancaStatus)
            .Include(cobranca => cobranca.HistoricoPagamentos)
                .ThenInclude(historico => historico.FormaPagamento)
            .AsNoTracking()
            .FirstOrDefaultAsync(cobranca => cobranca.Id == id);

        if (cobranca is null)
            return NotFound();

        return View(cobranca);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GerarCobrancasMensais()
    {
        var hoje = DateTime.Today;

        var caixasAtivas = await Contexto.CaixasPostais
            .Where(caixa => caixa.IdStatusCaixa == (int)CaixaStatusEnum.Ativa)
            .AsNoTracking()
            .ToListAsync();

        var cobrancasCriadas = 0;

        foreach (var caixa in caixasAtivas)
        {
            var jaExisteCobranca = await Contexto.Cobrancas
                .AnyAsync(cobranca =>
                    cobranca.IdCaixaPostal == caixa.Id
                    && cobranca.DataVencimento.Month == hoje.Month
                    && cobranca.DataVencimento.Year == hoje.Year);

            if (jaExisteCobranca)
                continue;

            var diaVencimento = Math.Min(caixa.DiaVencimento, DateTime.DaysInMonth(hoje.Year, hoje.Month));

            var novaCobranca = new Cobranca
            {
                IdCaixaPostal = caixa.Id,
                IdStatusCobranca = (short)CobrancaStatusEnum.Pendente,
                Valor = caixa.ValorMensal,
                DataVencimento = new DateTime(hoje.Year, hoje.Month, diaVencimento)
            };

            Contexto.Cobrancas.Add(novaCobranca);
            cobrancasCriadas++;
        }

        await Contexto.SaveChangesAsync();

        TempData["Sucesso"] = cobrancasCriadas > 0
            ? $"{cobrancasCriadas} cobrança(s) gerada(s) com sucesso!"
            : "Todas as cobranças do mês já foram geradas.";

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Excluir(int id)
    {
        var cobranca = await Contexto.Cobrancas
            .Include(c => c.CaixaPostal)
                .ThenInclude(cx => cx!.Cliente)
            .Include(c => c.CobrancaStatus)
            .Include(c => c.HistoricoPagamentos)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cobranca is null)
            return NotFound();

        return View(cobranca);
    }

    [HttpPost, ActionName("Excluir")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExcluirConfirmado(int id)
    {
        var cobranca = await Contexto.Cobrancas
            .Include(c => c.HistoricoPagamentos)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cobranca is null)
            return NotFound();

        Contexto.HistoricoPagamentos.RemoveRange(cobranca.HistoricoPagamentos);
        Contexto.Cobrancas.Remove(cobranca);
        await Contexto.SaveChangesAsync();

        TempData["Sucesso"] = "Cobrança excluída com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    private async Task AtualizarCobrancasAtrasadas()
    {
        var hoje = DateTime.Today;

        var cobrancasPendentesVencidas = await Contexto.Cobrancas
            .Where(cobranca =>
                cobranca.IdStatusCobranca == (short)CobrancaStatusEnum.Pendente
                && cobranca.DataVencimento < hoje)
            .ToListAsync();

        foreach (var cobranca in cobrancasPendentesVencidas)
            cobranca.IdStatusCobranca = (short)CobrancaStatusEnum.Atrasado;

        if (cobrancasPendentesVencidas.Count > 0)
            await Contexto.SaveChangesAsync();
    }
}
