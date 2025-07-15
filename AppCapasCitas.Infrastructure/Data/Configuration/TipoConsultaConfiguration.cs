using System;
using AppCapasCitas.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCapasCitas.Infrastructure.Data.Configuration;

public class TipoConsultaConfiguration : IEntityTypeConfiguration<TipoConsulta>
{
    public void Configure(EntityTypeBuilder<TipoConsulta> entity)
    {
        entity.ToTable("tipo_consulta");
        entity.HasKey(tc => tc.Id);
        entity.Property(tc => tc.Nombre)
            .IsRequired()
            .HasMaxLength(100);
         // ✅ AGREGAR: Configuración para MultiplicadorCosto
        entity.Property(tc => tc.MultiplicadorCosto)
            .HasColumnType("decimal(4,2)") // 0.10 a 99.99
            .HasDefaultValue(1.0M)
            .IsRequired();
    }
}
