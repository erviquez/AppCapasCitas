using System;
using AppCapasCitas.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCapasCitas.Infrastructure.Data.Configuration;

public class CitaConfiguration : IEntityTypeConfiguration<Cita>
{
    public void Configure(EntityTypeBuilder<Cita> entity)
    {
        // Relación con Paciente:
        // - Un paciente puede tener múltiples citas
        entity.HasOne(c => c.Paciente)
                .WithMany(p => p.Citas)
                .HasForeignKey(c => c.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);

        // Relación con Medico:
        // - Un médico puede tener múltiples citas programadas
        entity.HasOne(c => c.Medico)
                .WithMany(m => m.Citas)
                .HasForeignKey(c => c.MedicoId)
                .OnDelete(DeleteBehavior.Restrict);

        // Relación con Consultorio (opcional):
        // - Una cita puede tener un consultorio asignado
        // - Si se elimina el consultorio, la cita permanece (SetNull)
        entity.HasOne(c => c.Consultorio)
                .WithMany(co => co.Citas)
                .HasForeignKey(c => c.ConsultorioId)
                .OnDelete(DeleteBehavior.SetNull);

        // Relación con Pago (1-a-1 opcional):
        // - Una cita puede tener un pago asociado
        entity.HasOne(c => c.Pago)
                .WithOne(p => p.Cita)
                .HasForeignKey<Pago>(p => p.CitaId)
                .OnDelete(DeleteBehavior.SetNull);
        // Configuración de índices para mejorar el rendimiento de búsquedas frecuentes        
        entity.HasIndex(c => c.FechaHora); // Búsqueda por fecha
        entity.HasIndex(c => c.Estado); // Filtrado por estado de cita

        
    }
}
