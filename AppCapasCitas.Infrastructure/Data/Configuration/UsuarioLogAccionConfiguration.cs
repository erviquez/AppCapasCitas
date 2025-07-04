using System;
using AppCapasCitas.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCapasCitas.Infrastructure.Data.Configuration;

public class UsuarioLogAccionConfiguration : IEntityTypeConfiguration<UsuarioLogAccion>
{
    public void Configure(EntityTypeBuilder<UsuarioLogAccion> entity)
    {
        // Configuración de la entidad UsuarioLogAccion
        // Relación con Usuario:      
        entity
            .HasOne(u => u.UsuarioNavigation)
            .WithMany()
            .HasForeignKey(u => u.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);    
        // Relación con TipoAccion:
        entity
            .HasOne(t => t.TipoAccionNavigation)
            .WithMany()
            .HasForeignKey(t => t.TipoAccionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
