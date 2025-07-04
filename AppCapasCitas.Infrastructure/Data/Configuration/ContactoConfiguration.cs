using AppCapasCitas.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCapasCitas.Infrastructure.Data.Configuration;

public class ContactoConfiguration : IEntityTypeConfiguration<Contacto>
{
    public void Configure(EntityTypeBuilder<Contacto> entity)
    {
        // Relación con Paciente:
        // - Un paciente puede tener múltiples contactos
        // - Eliminación en cascada: si se elimina el paciente, se eliminan sus contactos
        entity.HasOne(c => c.PacienteNavigation)
              .WithMany(p => p.Contactos)
              .HasForeignKey(c => c.PacienteId)
              .OnDelete(DeleteBehavior.Cascade);

        // Configuración de propiedades adicionales si es necesario
        entity.Property(c => c.Nombre)
              .IsRequired()
              .HasMaxLength(100);

        entity.Property(c => c.Telefono)
              .IsRequired()
              .HasMaxLength(15);
    }
}
