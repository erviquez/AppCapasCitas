using System;
using AppCapasCitas.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCapasCitas.Infrastructure.Data.Configuration;

public class PagoConfiguration : IEntityTypeConfiguration<Pago>
{
    public void Configure(EntityTypeBuilder<Pago> entity)
    {
        // Relación con Paciente:
        // - Un paciente puede tener múltiples pagos registrados
        entity.HasOne(p => p.Paciente)
                .WithMany(pa => pa.Pagos)
                .HasForeignKey(p => p.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);

        // Configuración de monto:
        entity.Property(h => h.Monto)
            .HasColumnType("decimal(5,2)"); // Formato: $999.99

        // Configuración de índices para mejorar el rendimiento de búsquedas frecuentes        
        entity.HasIndex(p => p.Estado); // Filtrado por estado de pago

        
    }
}
