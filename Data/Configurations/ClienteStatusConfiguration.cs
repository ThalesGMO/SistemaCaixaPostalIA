using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaCaixaPostalIA.Models;

namespace SistemaCaixaPostalIA.Data.Configurations;

public class ClienteStatusConfiguration : IEntityTypeConfiguration<ClienteStatus>
{
    public void Configure(EntityTypeBuilder<ClienteStatus> builder)
    {
        builder.ToTable("ClientesStatus");

        builder.HasKey(status => status.Id);

        builder.Property(status => status.Id)
            .HasColumnName("Id")
            .ValueGeneratedNever();

        builder.Property(status => status.Nome)
            .HasColumnName("Nome")
            .IsRequired()
            .HasMaxLength(100);

        builder.HasData(
            new ClienteStatus { Id = 1, Nome = "Ativo" },
            new ClienteStatus { Id = 2, Nome = "Inativo" },
            new ClienteStatus { Id = 3, Nome = "Inadimplente" }
        );
    }
}
