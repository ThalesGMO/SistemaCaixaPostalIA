using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaCaixaPostalIA.Data;
using SistemaCaixaPostalIA.Models.Enums;
using SistemaCaixaPostalIA.ViewModels;

namespace SistemaCaixaPostalIA.Controllers;

public class HomeController : ControladorBase
{
    public HomeController(AppDbContext contexto) : base(contexto) { }

    public async Task<IActionResult> Index()
    {
        var idSocio = ObterIdSocioFiltro();
        var hoje = DateTime.Today;
        var inicioMes = new DateTime(hoje.Year, hoje.Month, 1);
        var fimMes = inicioMes.AddMonths(1).AddDays(-1);

        var viewModel = new DashboardViewModel
        {
            TotalRecebido = await CalcularTotalRecebido(idSocio, inicioMes, fimMes),
            TotalPendente = await CalcularTotalPorStatus(idSocio, CobrancaStatusEnum.Pendente),
            TotalAtrasado = await CalcularTotalPorStatus(idSocio, CobrancaStatusEnum.Atrasado),
            QuantidadeCaixasAtivas = await ContarCaixasAtivas(idSocio),
            CobrancasPendentes = await ContarCobrancasPorStatus(idSocio, CobrancaStatusEnum.Pendente),
            CobrancasPagas = await ContarCobrancasPorStatus(idSocio, CobrancaStatusEnum.Pago),
            CobrancasAtrasadas = await ContarCobrancasPorStatus(idSocio, CobrancaStatusEnum.Atrasado),
            ProximosVencimentos = await BuscarProximosVencimentos(idSocio, hoje),
            CobrancasEmAtraso = await BuscarCobrancasEmAtraso(idSocio, hoje)
        };

        PreencherDadosGraficoMensal(viewModel, idSocio, hoje);

        return View(viewModel);
    }

    private async Task<decimal> CalcularTotalRecebido(short? idSocio, DateTime inicioMes, DateTime fimMes)
    {
        var consulta = Contexto.HistoricoPagamentos
            .Include(historico => historico.Cobranca)
                .ThenInclude(cobranca => cobranca!.CaixaPostal)
            .Where(historico => historico.DataPagamento >= inicioMes && historico.DataPagamento <= fimMes)
            .AsNoTracking();

        if (idSocio.HasValue)
            consulta = consulta.Where(historico => historico.Cobranca!.CaixaPostal!.IdSocio == idSocio.Value);

        return await consulta.SumAsync(historico => historico.ValorPago);
    }

    private async Task<decimal> CalcularTotalPorStatus(short? idSocio, CobrancaStatusEnum status)
    {
        var consulta = Contexto.Cobrancas
            .Include(cobranca => cobranca.CaixaPostal)
            .Where(cobranca => cobranca.IdStatusCobranca == (short)status)
            .AsNoTracking();

        if (idSocio.HasValue)
            consulta = consulta.Where(cobranca => cobranca.CaixaPostal!.IdSocio == idSocio.Value);

        return await consulta.SumAsync(cobranca => cobranca.Valor);
    }

    private async Task<int> ContarCaixasAtivas(short? idSocio)
    {
        var consulta = Contexto.CaixasPostais
            .Where(caixa => caixa.IdStatusCaixa == (int)CaixaStatusEnum.Ativa)
            .AsNoTracking();

        if (idSocio.HasValue)
            consulta = consulta.Where(caixa => caixa.IdSocio == idSocio.Value);

        return await consulta.CountAsync();
    }

    private async Task<int> ContarCobrancasPorStatus(short? idSocio, CobrancaStatusEnum status)
    {
        var consulta = Contexto.Cobrancas
            .Include(cobranca => cobranca.CaixaPostal)
            .Where(cobranca => cobranca.IdStatusCobranca == (short)status)
            .AsNoTracking();

        if (idSocio.HasValue)
            consulta = consulta.Where(cobranca => cobranca.CaixaPostal!.IdSocio == idSocio.Value);

        return await consulta.CountAsync();
    }

    private async Task<List<CobrancaAlertaViewModel>> BuscarProximosVencimentos(short? idSocio, DateTime hoje)
    {
        var limiteSuperior = hoje.AddDays(7);

        var consulta = Contexto.Cobrancas
            .Include(cobranca => cobranca.CaixaPostal)
                .ThenInclude(caixa => caixa!.Cliente)
            .Include(cobranca => cobranca.CobrancaStatus)
            .Where(cobranca => cobranca.IdStatusCobranca == (short)CobrancaStatusEnum.Pendente)
            .Where(cobranca => cobranca.DataVencimento >= hoje && cobranca.DataVencimento <= limiteSuperior)
            .AsNoTracking();

        if (idSocio.HasValue)
            consulta = consulta.Where(cobranca => cobranca.CaixaPostal!.IdSocio == idSocio.Value);

        return await consulta
            .OrderBy(cobranca => cobranca.DataVencimento)
            .Select(cobranca => new CobrancaAlertaViewModel
            {
                CodigoCaixa = cobranca.CaixaPostal!.Codigo,
                NomeCliente = cobranca.CaixaPostal.Cliente!.Nome,
                DataVencimento = cobranca.DataVencimento,
                Valor = cobranca.Valor,
                NomeStatus = cobranca.CobrancaStatus!.Nome
            })
            .ToListAsync();
    }

    private async Task<List<CobrancaAlertaViewModel>> BuscarCobrancasEmAtraso(short? idSocio, DateTime hoje)
    {
        var consulta = Contexto.Cobrancas
            .Include(cobranca => cobranca.CaixaPostal)
                .ThenInclude(caixa => caixa!.Cliente)
            .Include(cobranca => cobranca.CobrancaStatus)
            .Where(cobranca => cobranca.IdStatusCobranca == (short)CobrancaStatusEnum.Atrasado)
            .AsNoTracking();

        if (idSocio.HasValue)
            consulta = consulta.Where(cobranca => cobranca.CaixaPostal!.IdSocio == idSocio.Value);

        return await consulta
            .OrderBy(cobranca => cobranca.DataVencimento)
            .Select(cobranca => new CobrancaAlertaViewModel
            {
                CodigoCaixa = cobranca.CaixaPostal!.Codigo,
                NomeCliente = cobranca.CaixaPostal.Cliente!.Nome,
                DataVencimento = cobranca.DataVencimento,
                Valor = cobranca.Valor,
                NomeStatus = cobranca.CobrancaStatus!.Nome
            })
            .ToListAsync();
    }

    private void PreencherDadosGraficoMensal(DashboardViewModel viewModel, short? idSocio, DateTime hoje)
    {
        for (var indice = 5; indice >= 0; indice--)
        {
            var mesReferencia = hoje.AddMonths(-indice);
            var inicioMes = new DateTime(mesReferencia.Year, mesReferencia.Month, 1);
            var fimMes = inicioMes.AddMonths(1).AddDays(-1);

            var consulta = Contexto.HistoricoPagamentos
                .Include(historico => historico.Cobranca)
                    .ThenInclude(cobranca => cobranca!.CaixaPostal)
                .Where(historico => historico.DataPagamento >= inicioMes && historico.DataPagamento <= fimMes)
                .AsNoTracking();

            if (idSocio.HasValue)
                consulta = consulta.Where(historico => historico.Cobranca!.CaixaPostal!.IdSocio == idSocio.Value);

            var totalMes = consulta.Sum(historico => historico.ValorPago);

            viewModel.RotulosMeses.Add(inicioMes.ToString("MMM/yy"));
            viewModel.ValoresPorMes.Add(totalMes);
        }
    }
}
