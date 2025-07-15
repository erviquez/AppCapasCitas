using System;
using AppCapasCitas.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCapasCitas.Infrastructure.Data.Configuration;

public class CitaConfiguration : IEntityTypeConfiguration<Cita>
{
        public void Configure(EntityTypeBuilder<Cita> entity)
        {
                // ✅ AGREGAR: Configuración de propiedades decimales
                entity.Property(c => c.CostoConsulta)
                        .HasColumnType("decimal(10,2)") // Hasta $99,999.99
                        .IsRequired(false); // Es opcional

                
                // Relación con Paciente:
                // - Un paciente puede tener múltiples citas
                entity.HasOne(c => c.Paciente)
                        .WithMany(p => p.Citas)
                        .HasForeignKey(c => c.PacienteId)
                        .OnDelete(DeleteBehavior.Restrict);

                // Relación con Medico:
                // - Un médico puede tener múltiples citas programadas
                entity.HasOne(c => c.MedicoNavigation)
                        .WithMany(m => m.Citas)
                        .HasForeignKey(c => c.MedicoId)
                        .OnDelete(DeleteBehavior.Restrict);

                // Relación con Consultorio (opcional):
                // - Una cita puede tener un consultorio asignado
                // - Si se elimina el consultorio, la cita permanece (SetNull)
                entity.HasOne(c => c.ConsultorioNavigation)
                        .WithMany(co => co.Citas)
                        .HasForeignKey(c => c.ConsultorioId)
                        .OnDelete(DeleteBehavior.SetNull);

                // Relación con Pago (1-a-1 opcional):
                // - Una cita puede tener un pago asociado
                entity.HasOne(c => c.PagoNavigation)
                        .WithOne(p => p.CitaNavigation)
                        .HasForeignKey<Pago>(p => p.CitaId)
                        .OnDelete(DeleteBehavior.SetNull);
                // Configuración de índices para mejorar el rendimiento de búsquedas frecuentes        
                entity.HasIndex(c => c.FechaHora); // Búsqueda por fecha
                entity.HasIndex(c => c.Estado); // Filtrado por estado de cita

                //Relacion con TipoConsulta (opcional):
                // - Una cita puede tener un tipo de consulta asociado
                entity.HasOne(c => c.TipoConsultaNavigation)
                        .WithMany(tc => tc.Citas)
                        .HasForeignKey(c => c.TipoConsultaId)
                        .OnDelete(DeleteBehavior.SetNull);

        
    }
}
