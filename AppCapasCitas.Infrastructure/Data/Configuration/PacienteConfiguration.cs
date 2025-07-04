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
        entity.HasOne(u => u.UsuarioNavigation)
              .WithOne(p => p.PacienteNavigation)
              .HasForeignKey<Paciente>(p => p.Id)
              .IsRequired(false)
              .OnDelete(DeleteBehavior.Restrict);
    }
}
