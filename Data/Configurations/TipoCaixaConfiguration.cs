using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaCaixaPostalIA.Models;

namespace SistemaCaixaPostalIA.Data.Configurations;

public class TipoCaixaConfiguration : IEntityTypeConfiguration<TipoCaixa>
{
    public void Configure(EntityTypeBuilder<TipoCaixa> builder)
    {
        builder.ToTable("TiposCaixa");

        builder.HasKey(tipo => tipo.Id);

        builder.Property(tipo => tipo.Id)
            .HasColumnName("Id")
            .ValueGeneratedNever();

        builder.Property(tipo => tipo.Nome)
            .HasColumnName("Nome")
            .IsRequired()
            .HasMaxLength(50);

        builder.HasData(
            new TipoCaixa { Id = 1, Nome = "Anuidade" },
            new TipoCaixa { Id = 2, Nome = "Cortesia" }
        );
    }
}
