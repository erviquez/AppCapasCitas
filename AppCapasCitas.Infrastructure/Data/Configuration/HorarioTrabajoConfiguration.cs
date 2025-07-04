using System;
using AppCapasCitas.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCapasCitas.Infrastructure.Data.Configuration;

public class HorarioTrabajoConfiguration : IEntityTypeConfiguration<HorarioTrabajo>
{
    public void Configure(EntityTypeBuilder<HorarioTrabajo> entity)
    {
        // Relación con Médico:
        // - Un médico puede tener múltiples horarios de trabajo
        // - Eliminación en cascada: si se elimina el médico, se eliminan sus horarios
        entity.HasOne(ht => ht.MedicoNavigation)
                .WithMany(m => m.HorariosTrabajo)
                .HasForeignKey(ht => ht.MedicoId)
                .OnDelete(DeleteBehavior.Cascade);
        
        entity .Property(h => h.DiaSemana)
                .HasConversion<int>(); // Guarda el enum como int en la DB

    }
}
