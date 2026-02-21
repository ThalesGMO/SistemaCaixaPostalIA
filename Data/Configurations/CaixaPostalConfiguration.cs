using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaCaixaPostalIA.Models;

namespace SistemaCaixaPostalIA.Data.Configurations;

public class CaixaPostalConfiguration : IEntityTypeConfiguration<CaixaPostal>
{
    public void Configure(EntityTypeBuilder<CaixaPostal> builder)
    {
        builder.ToTable("CaixasPostais");

        builder.HasKey(caixa => caixa.Id);

        builder.Property(caixa => caixa.Id)
            .HasColumnName("Id");

        builder.Property(caixa => caixa.IdSocio)
            .HasColumnName("IdSocio");

        builder.Property(caixa => caixa.IdCliente)
            .HasColumnName("IdCliente");

        builder.Property(caixa => caixa.IdStatusCaixa)
            .HasColumnName("IdStatusCaixa")
            .HasDefaultValue(1);

        builder.Property(caixa => caixa.IdTipoCaixa)
            .HasColumnName("IdTipoCaixa")
            .HasDefaultValue(1);

        builder.Property(caixa => caixa.Codigo)
            .HasColumnName("Codigo")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(caixa => caixa.NomeEmpresa)
            .HasColumnName("NomeEmpresa")
            .HasMaxLength(200);

        builder.Property(caixa => caixa.CpfCnpj)
            .HasColumnName("CpfCnpj")
            .IsRequired()
            .HasMaxLength(14);

        builder.Property(caixa => caixa.DataAluguel)
            .HasColumnName("DataAluguel")
            .HasColumnType("date")
            .IsRequired(false);

        builder.Property(caixa => caixa.DiaVencimento)
            .HasColumnName("DiaVencimento");

        builder.ToTable(tabela =>
            tabela.HasCheckConstraint("CK_CaixasPostais_DiaVencimento",
                "\"DiaVencimento\" BETWEEN 1 AND 31"));

        builder.Property(caixa => caixa.Valor)
            .HasColumnName("Valor")
            .HasColumnType("decimal(18,2)");

        builder.HasOne(caixa => caixa.Socio)
            .WithMany(socio => socio.CaixasPostais)
            .HasForeignKey(caixa => caixa.IdSocio)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(caixa => caixa.Cliente)
            .WithMany(cliente => cliente.CaixasPostais)
            .HasForeignKey(caixa => caixa.IdCliente)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(caixa => caixa.CaixaStatus)
            .WithMany()
            .HasForeignKey(caixa => caixa.IdStatusCaixa)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(caixa => caixa.TipoCaixa)
            .WithMany()
            .HasForeignKey(caixa => caixa.IdTipoCaixa)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
