using System;
using AppCapasCitas.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCapasCitas.Infrastructure.Data.Configuration;

public class MedicoEspecialidadHospitalConfiguration : IEntityTypeConfiguration<MedicoEspecialidadHospital>
{
    public void Configure(EntityTypeBuilder<MedicoEspecialidadHospital> entity)
    {
                // Definición de clave primaria
                // // entity.HasKey(meh => meh.Id);
                entity.HasKey(e => e.Id).HasName("pk_medico_especialidad_hospital");
                entity.ToTable("medico_especialidad_hospital"); 
                entity.HasIndex(e => e.CargoId, "ix_medico_especialidad_hospital_cargo_id");
                entity.HasIndex(e => e.EspecialidadId, "ix_medico_especialidad_hospital_especialidad_id");
                entity.HasIndex(e => e.HospitalId, "ix_medico_especialidad_hospital_hospital_id");

                entity.Property(m => m.CostoConsultaEspecifico)
                      .HasColumnType("decimal(10,2)")
                      .HasPrecision(10, 2);

                // Índice único para evitar duplicados en la combinación:
                // - Un médico no puede tener la misma especialidad en el mismo hospital múltiples veces
                // // entity.HasIndex(meh => new { meh.MedicoId, meh.EspecialidadId, meh.HospitalId }).IsUnique();
                entity.HasIndex(e => new { e.MedicoId, e.EspecialidadId, e.HospitalId }, "ix_medico_especialidad_hospital_medico_id_especialidad_id_hospital_id").IsUnique();


                 // Relación con Medico
                entity.HasOne(meh => meh.Medico)
                    .WithMany(m => m.MedicoEspecialidadHospitales)
                    .HasForeignKey(meh => meh.MedicoId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                // Relación con Especialidad
                entity.HasOne(meh => meh.Especialidad)
                    .WithMany(e => e.MedicoEspecialidadHospitales)
                    .HasForeignKey(meh => meh.EspecialidadId)
                    .OnDelete(DeleteBehavior.Restrict);
                    
                // Relación con Hospital
                entity.HasOne(meh => meh.Hospital)
                    .WithMany(h => h.MedicoEspecialidadHospitales)
                    .HasForeignKey(meh => meh.HospitalId)
                    .OnDelete(DeleteBehavior.Restrict);
    }
}
