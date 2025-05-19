
using Microsoft.EntityFrameworkCore;
using AppCapasCitas.Domain.Models;
using System.Reflection;
namespace AppCapasCitas.Infrastructure.Data;

public class InfrastructureDbContext:DbContext
{
    //     public DbSet<Medico> Medico { get; set; }
    // public DbSet<Paciente> Paciente { get; set; }
    // public DbSet<Usuario> Usuario { get; set; }
    // public DbSet<MedicoEspecialidadHospital> MedicoEspecialidadHospitales { get; set; }
    // public DbSet<Consultorio> Consultorio { get; set; }
    // public DbSet<HorarioTrabajo> HorariosTrabajo { get; set; }
    // public DbSet<Cita> Citas { get; set; }
    // public DbSet<HistorialMedico> HistorialMedico { get; set; }
    // public DbSet<RecetaMedica> RecetasMedicas { get; set; }
    // public DbSet<MedicamentoRecetado> MedicamentosRecetados { get; set; }
    // public DbSet<Pago> Pagos { get; set; }
    public InfrastructureDbContext(DbContextOptions<InfrastructureDbContext> options) : base(options)
    {
    }



     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        //base.OnModelCreating(modelBuilder);

            // Configuración principal de la entidad Usuario
            // modelBuilder.Entity<Usuario>(entity =>
            // {
            //     // Configuración de relación 1-a-1 con Paciente:
            //     // - Un Usuario puede tener asociado un Paciente (opcional)
            //     // - La eliminación está restringida para mantener la integridad referencial
            //     entity.HasOne(u => u.Paciente)
            //         .WithOne(p => p.Usuario)
            //         .HasForeignKey<Usuario>(u => u.PacienteId)
            //         .IsRequired(false)
            //         .OnDelete(DeleteBehavior.Restrict);

            //     // Configuración de relación 1-a-1 con Médico:
            //     // - Un Usuario puede tener asociado un Médico (opcional)
            //     // - Similar a Paciente pero para el rol de médico
            //     entity.HasOne(u => u.Medico)
            //         .WithOne(m => m.Usuario)
            //         .HasForeignKey<Usuario>(u => u.MedicoId)
            //         .IsRequired(false)
            //         .OnDelete(DeleteBehavior.Restrict);
            // });

            // Configuración de la entidad Paciente
            // modelBuilder.Entity<Paciente>(entity =>
            // {
            //     // Relación inversa con Usuario:
            //     // - Define el lado dependiente de la relación 1-a-1 con Usuario
            //     entity.HasOne(p => p.Usuario)
            //           .WithOne(u => u.Paciente)
            //           .HasForeignKey<Usuario>(u => u.PacienteId)
            //           .IsRequired(false)
            //           .OnDelete(DeleteBehavior.Restrict);
            // });

            // Configuración de la entidad Médico
            // modelBuilder.Entity<Medico>(entity =>
            // {
            //     // Relación inversa con Usuario:
            //     // - Similar a Paciente pero para médicos
            //     entity.HasOne(m => m.Usuario)
            //           .WithOne(u => u.Medico)
            //           .HasForeignKey<Usuario>(u => u.MedicoId)
            //           .IsRequired(false)
            //           .OnDelete(DeleteBehavior.Restrict);
            // });

            // Configuración de la entidad de unión MedicoEspecialidadHospital
            // modelBuilder.Entity<MedicoEspecialidadHospital>(entity =>
            // {
            //     // Definición de clave primaria
            //     // // entity.HasKey(meh => meh.Id);
            //     entity.HasKey(e => e.Id).HasName("pk_medico_especialidad_hospital");
            //     entity.ToTable("medico_especialidad_hospital"); 
            //     entity.HasIndex(e => e.CargoId, "ix_medico_especialidad_hospital_cargo_id");
            //     entity.HasIndex(e => e.EspecialidadId, "ix_medico_especialidad_hospital_especialidad_id");
            //     entity.HasIndex(e => e.HospitalId, "ix_medico_especialidad_hospital_hospital_id");

            //     entity.Property(m => m.CostoConsultaEspecifico)
            //           .HasColumnType("decimal(10,2)")
            //           .HasPrecision(10, 2);

            //     // Índice único para evitar duplicados en la combinación:
            //     // - Un médico no puede tener la misma especialidad en el mismo hospital múltiples veces
            //     // // entity.HasIndex(meh => new { meh.MedicoId, meh.EspecialidadId, meh.HospitalId }).IsUnique();
            //     entity.HasIndex(e => new { e.MedicoId, e.EspecialidadId, e.HospitalId }, "ix_medico_especialidad_hospital_medico_id_especialidad_id_hospital_id").IsUnique();


            //      // Relación con Medico
            //     entity.HasOne(meh => meh.Medico)
            //         .WithMany(m => m.MedicoEspecialidadHospitales)
            //         .HasForeignKey(meh => meh.MedicoId)
            //         .OnDelete(DeleteBehavior.Cascade);
                    
            //     // Relación con Especialidad
            //     entity.HasOne(meh => meh.Especialidad)
            //         .WithMany(e => e.MedicoEspecialidadHospitales)
            //         .HasForeignKey(meh => meh.EspecialidadId)
            //         .OnDelete(DeleteBehavior.Restrict);
                    
            //     // Relación con Hospital
            //     entity.HasOne(meh => meh.Hospital)
            //         .WithMany(h => h.MedicoEspecialidadHospitales)
            //         .HasForeignKey(meh => meh.HospitalId)
            //         .OnDelete(DeleteBehavior.Restrict);

            //     // // // Relación con Médico:
            //     // // // - Un médico puede tener múltiples especialidades en diferentes hospitales
            //     // // entity.HasOne(meh => meh.Medico)
            //     // //       .WithMany(m => m.MedicoEspecialidadHospitales)
            //     // //       .HasForeignKey(meh => meh.MedicoId)
            //     // //       .OnDelete(DeleteBehavior.Restrict);
                

            //     // // // Relación con Especialidad:
            //     // // // - Configuración de la especialidad médica
            //     // // entity.HasOne(meh => meh.Especialidad)
            //     // //       .WithMany()
            //     // //       .HasForeignKey(meh => meh.EspecialidadId)
            //     // //       .OnDelete(DeleteBehavior.Restrict);

            //     // // // Relación con Cargo:
            //     // // // - Define el puesto o cargo del médico en este contexto específico
            //     // // entity.HasOne(meh => meh.Cargo)
            //     // //       .WithMany()
            //     // //       .HasForeignKey(meh => meh.CargoId)
            //     // //       .OnDelete(DeleteBehavior.Restrict);

            //     // Configuración de propiedades decimales para costos:
            //     // - Precisión de 10 dígitos totales con 2 decimales (ej: 99999999.99)
                
            // });

            // Configuración de Consultorio
            // modelBuilder.Entity<Consultorio>(entity =>
            // {
            //     // Relación con Hospital:
            //     // - Un consultorio pertenece a un hospital
            //     // - Un hospital puede tener múltiples consultorios
            //     entity.HasOne(c => c.Hospital)
            //           .WithMany(h => h.Consultorios)
            //           .HasForeignKey(c => c.HospitalId)
            //           .OnDelete(DeleteBehavior.Restrict);
            // });

            // Configuración de HorarioTrabajo
            // modelBuilder.Entity<HorarioTrabajo>(entity =>
            // {
            //     // Relación con Médico:
            //     // - Un médico puede tener múltiples horarios de trabajo
            //     // - Eliminación en cascada: si se elimina el médico, se eliminan sus horarios
            //     entity.HasOne(ht => ht.Medico)
            //           .WithMany(m => m.HorariosTrabajo)
            //           .HasForeignKey(ht => ht.MedicoId)
            //           .OnDelete(DeleteBehavior.Cascade);
                
            // });
           
            // modelBuilder.Entity<HorarioTrabajo>()
            //     .Property(h => h.DiaSemana)
            //     .HasConversion<int>(); // Guarda el enum como int en la DB

            
            // // Configuración de Cita
            // modelBuilder.Entity<Cita>(entity =>
            // {
            //     // Relación con Paciente:
            //     // - Un paciente puede tener múltiples citas
            //     entity.HasOne(c => c.Paciente)
            //           .WithMany(p => p.Citas)
            //           .HasForeignKey(c => c.PacienteId)
            //           .OnDelete(DeleteBehavior.Restrict);

            //     // Relación con Medico:
            //     // - Un médico puede tener múltiples citas programadas
            //     entity.HasOne(c => c.Medico)
            //           .WithMany(m => m.Citas)
            //           .HasForeignKey(c => c.MedicoId)
            //           .OnDelete(DeleteBehavior.Restrict);

            //     // Relación con Consultorio (opcional):
            //     // - Una cita puede tener un consultorio asignado
            //     // - Si se elimina el consultorio, la cita permanece (SetNull)
            //     entity.HasOne(c => c.Consultorio)
            //           .WithMany(co => co.Citas)
            //           .HasForeignKey(c => c.ConsultorioId)
            //           .OnDelete(DeleteBehavior.SetNull);

            //     // Relación con Pago (1-a-1 opcional):
            //     // - Una cita puede tener un pago asociado
            //     entity.HasOne(c => c.Pago)
            //           .WithOne(p => p.Cita)
            //           .HasForeignKey<Pago>(p => p.CitaId)
            //           .OnDelete(DeleteBehavior.SetNull);
            // });

            // // Configuración de HistorialMedico
            // modelBuilder.Entity<HistorialMedico>(entity =>
            // {
            //     // Relación con Paciente:
            //     // - Un paciente tiene un historial médico que puede contener múltiples registros
            //     entity.HasOne(hm => hm.Paciente)
            //           .WithMany(p => p.HistorialMedico)
            //           .HasForeignKey(hm => hm.PacienteId)
            //           .OnDelete(DeleteBehavior.Restrict);

            //     // // // Relación con Medico:
            //     // // // - Registra qué médico realizó la entrada en el historial
            //     // // entity.HasOne(hm => hm.Medico)
            //     // //       .WithMany()
            //     // //       .HasForeignKey(hm => hm.MedicoId)
            //     // //       .OnDelete(DeleteBehavior.Restrict);

            //     // Relación con Cita (opcional):
            //     // - Vincula el registro del historial con una cita específica
            //     entity.HasOne(hm => hm.Cita)
            //           .WithMany()
            //           .HasForeignKey(hm => hm.CitaId)
            //           .OnDelete(DeleteBehavior.SetNull);

            //     // Configuración de propiedades médicas:
            //     entity.Property(h => h.Temperatura)
            //         .HasColumnType("decimal(5,2)"); // Formato: 999.99°C

            //     entity.Property(h => h.Peso)
            //         .HasColumnType("decimal(5,2)"); // Formato: 999.99 kg

            //     entity.Property(h => h.Altura)
            //         .HasColumnType("decimal(4,2)"); // Formato: 2.50 metros
            // });

            // // Configuración de RecetaMedica
            // modelBuilder.Entity<RecetaMedica>(entity =>
            // {
            //     // Relación con Médico:
            //     // - Un médico puede emitir múltiples recetas
            //     entity.HasOne(rm => rm.Medico)
            //           .WithMany(m => m.RecetasMedicas)
            //           .HasForeignKey(rm => rm.MedicoId)
            //           .OnDelete(DeleteBehavior.Restrict);

            //     // Relación con Paciente:
            //     // - Un paciente puede tener múltiples recetas
            //     entity.HasOne(rm => rm.Paciente)
            //           .WithMany(p => p.RecetasMedicas)
            //           .HasForeignKey(rm => rm.PacienteId)
            //           .OnDelete(DeleteBehavior.Restrict);

            //     // Relación con Cita (opcional):
            //     // - Una receta puede estar asociada a una cita específica
            //     entity.HasOne(rm => rm.Cita)
            //           .WithMany(c => c.RecetasMedicas)
            //           .HasForeignKey(rm => rm.CitaId)
            //           .OnDelete(DeleteBehavior.SetNull);
            // });

            // // Configuración de MedicamentoRecetado
            // modelBuilder.Entity<MedicamentoRecetado>(entity =>
            // {
            //     // Relación con RecetaMedica:
            //     // - Una receta puede contener múltiples medicamentos
            //     // - Eliminación en cascada: si se elimina la receta, se eliminan sus medicamentos
            //     entity.HasOne(mr => mr.RecetaMedica)
            //           .WithMany(rm => rm.Medicamentos)
            //           .HasForeignKey(mr => mr.RecetaMedicaId)
            //           .OnDelete(DeleteBehavior.Cascade);
            // });

            // // Configuración de Pago
            // modelBuilder.Entity<Pago>(entity =>
            // {
            //     // Relación con Paciente:
            //     // - Un paciente puede tener múltiples pagos registrados
            //     entity.HasOne(p => p.Paciente)
            //           .WithMany(pa => pa.Pagos)
            //           .HasForeignKey(p => p.PacienteId)
            //           .OnDelete(DeleteBehavior.Restrict);

            //     // Configuración de monto:
            //     entity.Property(h => h.Monto)
            //         .HasColumnType("decimal(5,2)"); // Formato: $999.99
            // });

            // Configuración de índices para mejorar el rendimiento de búsquedas frecuentes
            //modelBuilder.Entity<Cita>().HasIndex(c => c.FechaHora); // Búsqueda por fecha
            //modelBuilder.Entity<Cita>().HasIndex(c => c.Estado); // Filtrado por estado de cita
            //modelBuilder.Entity<RecetaMedica>().HasIndex(rm => rm.FechaEmision); // Búsqueda por fecha de emisión
            //modelBuilder.Entity<Pago>().HasIndex(p => p.Estado); // Filtrado por estado de pago

            // Índice único para el IdentityId (asumiendo integración con sistema de autenticación)
           // modelBuilder.Entity<Usuario>().HasIndex(u => u.IdentityId).IsUnique();
       
    }


}

