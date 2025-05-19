using System;
using AppCapasCitas.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCapasCitas.Infrastructure.Data.Configuration;

public class ConsultorioConfiguration : IEntityTypeConfiguration<Consultorio>
{
    public void Configure(EntityTypeBuilder<Consultorio> entity)
    {
        // Relación con Hospital:
        // - Un consultorio pertenece a un hospital
        // - Un hospital puede tener múltiples consultorios
        entity.HasOne(c => c.Hospital)
                .WithMany(h => h.Consultorios)
                .HasForeignKey(c => c.HospitalId)
                .OnDelete(DeleteBehavior.Restrict);
    }
}
