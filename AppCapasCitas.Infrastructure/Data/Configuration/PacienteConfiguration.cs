using System;
using AppCapasCitas.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCapasCitas.Infrastructure.Data.Configuration;

public class PacienteConfiguration : IEntityTypeConfiguration<Paciente>
{
    public void Configure(EntityTypeBuilder<Paciente> entity)
    {
        // Relación inversa con Usuario:
        // - Define el lado dependiente de la relación 1-a-1 con Usuario
        entity.HasOne(p => p.Usuario)
                .WithOne(u => u.Paciente)
                .HasForeignKey<Usuario>(u => u.PacienteId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
    }
}
