using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaCaixaPostalIA.Models;

namespace SistemaCaixaPostalIA.Data.Configurations;

public class CobrancaConfiguration : IEntityTypeConfiguration<Cobranca>
{
    public void Configure(EntityTypeBuilder<Cobranca> builder)
    {
        builder.ToTable("Cobrancas");

        builder.HasKey(cobranca => cobranca.Id);

        builder.Property(cobranca => cobranca.Id)
            .HasColumnName("Id");

        builder.Property(cobranca => cobranca.IdCaixaPostal)
            .HasColumnName("IdCaixaPostal");

        builder.Property(cobranca => cobranca.IdStatusCobranca)
            .HasColumnName("IdStatusCobranca")
            .HasDefaultValue((short)1);

        builder.Property(cobranca => cobranca.Valor)
            .HasColumnName("Valor")
            .HasColumnType("decimal(18,2)");

        builder.Property(cobranca => cobranca.DataVencimento)
            .HasColumnName("DataVencimento")
            .HasColumnType("date");

        builder.Property(cobranca => cobranca.DataLiquidacao)
            .HasColumnName("DataLiquidacao")
            .HasColumnType("date");

        builder.HasOne(cobranca => cobranca.CaixaPostal)
            .WithMany(caixa => caixa.Cobrancas)
            .HasForeignKey(cobranca => cobranca.IdCaixaPostal)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cobranca => cobranca.CobrancaStatus)
            .WithMany()
            .HasForeignKey(cobranca => cobranca.IdStatusCobranca)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
