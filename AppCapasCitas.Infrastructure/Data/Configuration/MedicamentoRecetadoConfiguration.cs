using System;
using AppCapasCitas.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCapasCitas.Infrastructure.Data.Configuration;

public class MedicamentoRecetadoConfiguration : IEntityTypeConfiguration<MedicamentoRecetado>
{
    public void Configure(EntityTypeBuilder<MedicamentoRecetado> entity)
    {
        // Relación con RecetaMedica:
        // - Una receta puede contener múltiples medicamentos
        // - Eliminación en cascada: si se elimina la receta, se eliminan sus medicamentos
        entity.HasOne(mr => mr.RecetaMedicaNavigation)
                .WithMany(rm => rm.Medicamentos)
                .HasForeignKey(mr => mr.RecetaMedicaId)
                .OnDelete(DeleteBehavior.Cascade);
    }
}
