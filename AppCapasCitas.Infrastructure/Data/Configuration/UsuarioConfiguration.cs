using System;
using AppCapasCitas.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCapasCitas.Infrastructure.Data.Configuration;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> entity)
    {
        // Configuración de relación 1-a-1 con Paciente:
        // - Un Usuario puede tener asociado un Paciente (opcional)
        // - La eliminación está restringida para mantener la integridad referencial
        entity.HasOne(u => u.Paciente)
            .WithOne(p => p.Usuario)
            .HasForeignKey<Usuario>(u => u.PacienteId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        // Configuración de relación 1-a-1 con Médico:
        // - Un Usuario puede tener asociado un Médico (opcional)
        // - Similar a Paciente pero para el rol de médico
        entity.HasOne(u => u.Medico)
            .WithOne(m => m.Usuario)
            .HasForeignKey<Usuario>(u => u.MedicoId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        // Índice único para el IdentityId (asumiendo integración con sistema de autenticación)
        entity.HasIndex(u => u.IdentityId).IsUnique();

    }
}
