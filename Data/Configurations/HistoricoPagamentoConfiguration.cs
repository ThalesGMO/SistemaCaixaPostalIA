using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaCaixaPostalIA.Models;

namespace SistemaCaixaPostalIA.Data.Configurations;

public class HistoricoPagamentoConfiguration : IEntityTypeConfiguration<HistoricoPagamento>
{
    public void Configure(EntityTypeBuilder<HistoricoPagamento> builder)
    {
        builder.ToTable("HistoricoPagamentos");

        builder.HasKey(historico => historico.Id);

        builder.Property(historico => historico.Id)
            .HasColumnName("Id");

        builder.Property(historico => historico.IdCobranca)
            .HasColumnName("IdCobranca");

        builder.Property(historico => historico.IdFormaPagamento)
            .HasColumnName("IdFormaPagamento");

        builder.Property(historico => historico.DataPagamento)
            .HasColumnName("DataPagamento")
            .HasColumnType("date")
            .HasDefaultValueSql("CURRENT_DATE");

        builder.Property(historico => historico.ValorPago)
            .HasColumnName("ValorPago")
            .HasColumnType("decimal(18,2)");

        builder.Property(historico => historico.Observacao)
            .HasColumnName("Observacao")
            .HasMaxLength(400);

        builder.HasOne(historico => historico.Cobranca)
            .WithMany(cobranca => cobranca.HistoricoPagamentos)
            .HasForeignKey(historico => historico.IdCobranca)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(historico => historico.FormaPagamento)
            .WithMany()
            .HasForeignKey(historico => historico.IdFormaPagamento)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
