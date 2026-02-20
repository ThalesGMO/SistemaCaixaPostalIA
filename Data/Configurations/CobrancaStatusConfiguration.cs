using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaCaixaPostalIA.Models;

namespace SistemaCaixaPostalIA.Data.Configurations;

public class CobrancaStatusConfiguration : IEntityTypeConfiguration<CobrancaStatus>
{
    public void Configure(EntityTypeBuilder<CobrancaStatus> builder)
    {
        builder.ToTable("CobrancasStatus");

        builder.HasKey(status => status.Id);

        builder.Property(status => status.Id)
            .HasColumnName("Id")
            .ValueGeneratedNever();

        builder.Property(status => status.Nome)
            .HasColumnName("Nome")
            .IsRequired()
            .HasMaxLength(50);

        builder.HasData(
            new CobrancaStatus { Id = 1, Nome = "Pendente" },
            new CobrancaStatus { Id = 2, Nome = "Pago" },
            new CobrancaStatus { Id = 3, Nome = "Atrasado" },
            new CobrancaStatus { Id = 4, Nome = "Cancelado" }
        );
    }
}
