using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaCaixaPostalIA.Models;

namespace SistemaCaixaPostalIA.Data.Configurations;

public class FormaPagamentoConfiguration : IEntityTypeConfiguration<FormaPagamento>
{
    public void Configure(EntityTypeBuilder<FormaPagamento> builder)
    {
        builder.ToTable("FormaPagamentos");

        builder.HasKey(forma => forma.Id);

        builder.Property(forma => forma.Id)
            .HasColumnName("Id")
            .ValueGeneratedNever();

        builder.Property(forma => forma.Nome)
            .HasColumnName("Nome")
            .IsRequired()
            .HasMaxLength(50);

        builder.HasData(
            new FormaPagamento { Id = 1, Nome = "Dinheiro" },
            new FormaPagamento { Id = 2, Nome = "PIX" },
            new FormaPagamento { Id = 3, Nome = "Boleto" }
        );
    }
}
