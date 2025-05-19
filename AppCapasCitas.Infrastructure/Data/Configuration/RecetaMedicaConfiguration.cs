using System;
using AppCapasCitas.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCapasCitas.Infrastructure.Data.Configuration;

public class RecetaMedicaConfiguration : IEntityTypeConfiguration<RecetaMedica>
{
    public void Configure(EntityTypeBuilder<RecetaMedica> entity)
    {
        // Relación con Médico:
        // - Un médico puede emitir múltiples recetas
        entity.HasOne(rm => rm.Medico)
                .WithMany(m => m.RecetasMedicas)
                .HasForeignKey(rm => rm.MedicoId)
                .OnDelete(DeleteBehavior.Restrict);

        // Relación con Paciente:
        // - Un paciente puede tener múltiples recetas
        entity.HasOne(rm => rm.Paciente)
                .WithMany(p => p.RecetasMedicas)
                .HasForeignKey(rm => rm.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);

        // Relación con Cita (opcional):
        // - Una receta puede estar asociada a una cita específica
        entity.HasOne(rm => rm.Cita)
                .WithMany(c => c.RecetasMedicas)
                .HasForeignKey(rm => rm.CitaId)
                .OnDelete(DeleteBehavior.SetNull);
        // Configuración de índices para mejorar el rendimiento de búsquedas frecuentes
        entity.HasIndex(rm => rm.FechaEmision); // Búsqueda por fecha de emisión

        


        
    }
}
