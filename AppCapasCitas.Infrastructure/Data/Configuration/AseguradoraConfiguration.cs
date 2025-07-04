using AppCapasCitas.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCapasCitas.Infrastructure.Data.Configuration;

public class AseguradoraConfiguration : IEntityTypeConfiguration<Aseguradora>
{
    public void Configure(EntityTypeBuilder<Aseguradora> entity)
    {

           entity.HasOne(p => p.PacienteNavigation)
                .WithMany(m => m.Aseguradoras)
                .HasForeignKey(p => p.PacienteId)
                .OnDelete(DeleteBehavior.Cascade);
    }
}
