using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaCaixaPostalIA.Models;

namespace SistemaCaixaPostalIA.Data.Configurations;

public class SocioConfiguration : IEntityTypeConfiguration<Socio>
{
    public void Configure(EntityTypeBuilder<Socio> builder)
    {
        builder.ToTable("Socios");

        builder.HasKey(socio => socio.Id);

        builder.Property(socio => socio.Id)
            .HasColumnName("Id");

        builder.Property(socio => socio.Nome)
            .HasColumnName("Nome")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(socio => socio.Email)
            .HasColumnName("Email")
            .IsRequired()
            .HasMaxLength(320);

        builder.Property(socio => socio.Numero)
            .HasColumnName("Numero")
            .HasMaxLength(11);
    }
}
