using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaCaixaPostalIA.Data;
using SistemaCaixaPostalIA.Models;
using SistemaCaixaPostalIA.Models.Enums;
using SistemaCaixaPostalIA.ViewModels;

namespace SistemaCaixaPostalIA.Controllers;

public class HistoricoPagamentosController : ControladorBase
{
    public HistoricoPagamentosController(AppDbContext contexto) : base(contexto) { }

    public async Task<IActionResult> Index(int? cobrancaId)
    {
        var consulta = Contexto.HistoricoPagamentos
            .Include(historico => historico.Cobranca)
                .ThenInclude(cobranca => cobranca!.CaixaPostal)
                    .ThenInclude(caixa => caixa!.Cliente)
            .Include(historico => historico.FormaPagamento)
            .AsNoTracking();

        if (cobrancaId.HasValue)
            consulta = consulta.Where(historico => historico.IdCobranca == cobrancaId.Value);

        var historicos = await consulta
            .OrderByDescending(historico => historico.DataPagamento)
            .ToListAsync();

        ViewBag.CobrancaIdFiltro = cobrancaId;
        return View(historicos);
    }

    public async Task<IActionResult> Criar(int cobrancaId)
    {
        var cobranca = await Contexto.Cobrancas
            .Include(cobranca => cobranca.CaixaPostal)
                .ThenInclude(caixa => caixa!.Cliente)
            .AsNoTracking()
            .FirstOrDefaultAsync(cobranca => cobranca.Id == cobrancaId);

        if (cobranca is null)
            return NotFound();

        var viewModel = new HistoricoPagamentoViewModel
        {
            IdCobranca = cobranca.Id,
            DataPagamento = DateTime.Today,
            ValorPago = cobranca.Valor,
            CodigoCaixa = cobranca.CaixaPostal?.Codigo,
            NomeCliente = cobranca.CaixaPostal?.Cliente?.Nome,
            ValorCobranca = cobranca.Valor,
            ListaFormasPagamento = await ObterListaFormasPagamento()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Criar(HistoricoPagamentoViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            viewModel.ListaFormasPagamento = await ObterListaFormasPagamento();
            return View(viewModel);
        }

        var historico = new HistoricoPagamento
        {
            IdCobranca = viewModel.IdCobranca,
            IdFormaPagamento = viewModel.IdFormaPagamento,
            DataPagamento = viewModel.DataPagamento,
            ValorPago = viewModel.ValorPago,
            Observacao = viewModel.Observacao
        };

        Contexto.HistoricoPagamentos.Add(historico);
        await Contexto.SaveChangesAsync();

        await MarcarCobrancaComoPaga(viewModel.IdCobranca);

        TempData["Sucesso"] = "Pagamento registrado com sucesso!";
        return RedirectToAction("Detalhes", "Cobrancas", new { id = viewModel.IdCobranca });
    }

    private async Task MarcarCobrancaComoPaga(int idCobranca)
    {
        var cobranca = await Contexto.Cobrancas
            .Include(cobranca => cobranca.CaixaPostal)
                .ThenInclude(caixa => caixa!.Cliente)
            .FirstOrDefaultAsync(cobranca => cobranca.Id == idCobranca);

        if (cobranca is null)
            return;

        cobranca.IdStatusCobranca = (short)CobrancaStatusEnum.Pago;
        cobranca.DataLiquidacao = DateTime.Today;

        await ReverterInadimplenciaSeNecessario(cobranca.CaixaPostal!.IdCliente);
        await Contexto.SaveChangesAsync();
    }

    private async Task ReverterInadimplenciaSeNecessario(int idCliente)
    {
        var cliente = await Contexto.Clientes.FindAsync(idCliente);

        if (cliente is null)
            return;

        if (cliente.IdClienteStatus != (int)ClienteStatusEnum.Inadimplente)
            return;

        var possuiCobrancasEmAberto = await Contexto.Cobrancas
            .AnyAsync(cobranca =>
                cobranca.CaixaPostal!.IdCliente == idCliente
                && cobranca.IdStatusCobranca != (short)CobrancaStatusEnum.Pago
                && cobranca.IdStatusCobranca != (short)CobrancaStatusEnum.Cancelado
                && cobranca.DataVencimento < DateTime.Today.AddDays(-5));

        if (possuiCobrancasEmAberto)
            return;

        cliente.IdClienteStatus = (int)ClienteStatusEnum.Ativo;
    }

    private async Task<SelectList> ObterListaFormasPagamento()
    {
        var formas = await Contexto.FormaPagamentos
            .AsNoTracking()
            .OrderBy(forma => forma.Id)
            .ToListAsync();

        return new SelectList(formas, "Id", "Nome");
    }
}
