using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaCaixaPostalIA.Models;

namespace SistemaCaixaPostalIA.Data.Configurations;

public class CaixaStatusConfiguration : IEntityTypeConfiguration<CaixaStatus>
{
    public void Configure(EntityTypeBuilder<CaixaStatus> builder)
    {
        builder.ToTable("CaixasStatus");

        builder.HasKey(status => status.Id);

        builder.Property(status => status.Id)
            .HasColumnName("Id")
            .ValueGeneratedNever();

        builder.Property(status => status.Nome)
            .HasColumnName("Nome")
            .IsRequired()
            .HasMaxLength(50);

        builder.HasData(
            new CaixaStatus { Id = 1, Nome = "Ativa" },
            new CaixaStatus { Id = 2, Nome = "Bloqueada" },
            new CaixaStatus { Id = 3, Nome = "Cancelada" }
        );
    }
}
