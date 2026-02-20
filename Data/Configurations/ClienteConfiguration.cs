using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaCaixaPostalIA.Models;

namespace SistemaCaixaPostalIA.Data.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("Clientes");

        builder.HasKey(cliente => cliente.Id);

        builder.Property(cliente => cliente.Id)
            .HasColumnName("Id");

        builder.Property(cliente => cliente.IdClienteStatus)
            .HasColumnName("IdClienteStatus")
            .HasDefaultValue(1);

        builder.Property(cliente => cliente.Nome)
            .HasColumnName("Nome")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(cliente => cliente.Email)
            .HasColumnName("Email")
            .HasMaxLength(320);

        builder.Property(cliente => cliente.Telefone)
            .HasColumnName("Telefone")
            .HasMaxLength(11);

        builder.HasOne(cliente => cliente.ClienteStatus)
            .WithMany()
            .HasForeignKey(cliente => cliente.IdClienteStatus)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
