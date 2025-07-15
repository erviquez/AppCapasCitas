using System;
using AppCapasCitas.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCapasCitas.Infrastructure.Data.Configuration;


public class EspecialidadConfiguration : IEntityTypeConfiguration<Especialidad>
{
    public void Configure(EntityTypeBuilder<Especialidad> entity)
    {
        // Configuración de la tabla
        entity.ToTable("especialidad");
        entity.HasKey(e => e.Id);

        // Configuración de propiedades
        entity.Property(e => e.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        entity.Property(e => e.Descripcion)
            .HasMaxLength(1000);

        // ✅ CONFIGURACIÓN PARA CostoConsultaBase
        entity.Property(e => e.CostoConsultaBase)
            .HasColumnType("decimal(10,2)") // Hasta $99,999.99
            .IsRequired(false); // Es opcional

        // Configuración de índices
        entity.HasIndex(e => e.Nombre)
            .IsUnique()
            .HasDatabaseName("IX_Especialidad_Nombre"); // Nombre único

        entity.HasIndex(e => e.Activo)
            .HasDatabaseName("IX_Especialidad_Activo"); // Filtrar activos
    }
}
