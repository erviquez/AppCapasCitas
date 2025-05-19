using System;
using AppCapasCitas.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCapasCitas.Infrastructure.Data.Configuration;

public class HistorialMedicoConfiguration : IEntityTypeConfiguration<HistorialMedico>
{
    public void Configure(EntityTypeBuilder<HistorialMedico> entity)
    {
        // Relación con Paciente:
        // - Un paciente tiene un historial médico que puede contener múltiples registros
        entity.HasOne(hm => hm.Paciente)
                .WithMany(p => p.HistorialMedico)
                .HasForeignKey(hm => hm.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);

        // Relación con Cita (opcional):
        // - Vincula el registro del historial con una cita específica
        entity.HasOne(hm => hm.Cita)
                .WithMany()
                .HasForeignKey(hm => hm.CitaId)
                .OnDelete(DeleteBehavior.SetNull);

        // Configuración de propiedades médicas:
        entity.Property(h => h.Temperatura)
            .HasColumnType("decimal(5,2)"); // Formato: 999.99°C

        entity.Property(h => h.Peso)
            .HasColumnType("decimal(5,2)"); // Formato: 999.99 kg

        entity.Property(h => h.Altura)
            .HasColumnType("decimal(4,2)"); // Formato: 2.50 metros
    }
}
