using System;
using AppCapasCitas.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCapasCitas.Infrastructure.Data.Configuration;

public class TipoSangreConfiguration : IEntityTypeConfiguration<TipoSangre>
{
    public void Configure(EntityTypeBuilder<TipoSangre> entity)
    {
        // Configuración de la entidad TipoSangre
        // Aquí puedes definir las propiedades y relaciones de la entidad TipoSangre

        // Ejemplo: Definir un índice único para el nombre del tipo de sangre
        entity.HasIndex(ts => ts.Nombre).IsUnique();

        // Configurar la relación uno a uno con Paciente
        // - Un TipoSangre puede estar asociado a múltiples Pacientes
        entity.HasMany(ts => ts.Pacientes)
              .WithOne(p => p.TipoSangreNavigation)
              .HasForeignKey(p => p.TipoSangreId)
              .OnDelete(DeleteBehavior.Restrict);
    }

}
