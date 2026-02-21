using Microsoft.EntityFrameworkCore;
using SistemaCaixaPostalIA.Models;

namespace SistemaCaixaPostalIA.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opcoes) : base(opcoes) { }

    public DbSet<Socio> Socios => Set<Socio>();
    public DbSet<ClienteStatus> ClientesStatus => Set<ClienteStatus>();
    public DbSet<CaixaStatus> CaixasStatus => Set<CaixaStatus>();
    public DbSet<TipoCaixa> TiposCaixa => Set<TipoCaixa>();
    public DbSet<CobrancaStatus> CobrancasStatus => Set<CobrancaStatus>();
    public DbSet<FormaPagamento> FormaPagamentos => Set<FormaPagamento>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<CaixaPostal> CaixasPostais => Set<CaixaPostal>();
    public DbSet<Cobranca> Cobrancas => Set<Cobranca>();
    public DbSet<HistoricoPagamento> HistoricoPagamentos => Set<HistoricoPagamento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
