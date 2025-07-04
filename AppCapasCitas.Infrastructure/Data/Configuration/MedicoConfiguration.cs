using AppCapasCitas.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCapasCitas.Infrastructure.Data.Configuration;

public class MedicoConfiguration : IEntityTypeConfiguration<Medico>
{
    public void Configure(EntityTypeBuilder<Medico> entity)
    {
        // Configuración de la entidad Medico
        // Relación inversa con Usuario:
        // - Similar a Paciente pero para médicos
        entity.HasOne(u => u.UsuarioNavigation)
              .WithOne(m => m.MedicoNavigation)
              .HasForeignKey<Medico>(m => m.Id)
              .IsRequired(false)
              .OnDelete(DeleteBehavior.Restrict);


    }
}
