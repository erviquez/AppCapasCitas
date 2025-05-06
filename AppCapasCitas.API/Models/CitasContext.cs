using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AppCapasCitas.API.Models;

public partial class CitasContext : DbContext
{
    public CitasContext()
    {
    }

    public CitasContext(DbContextOptions<CitasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cargo> Cargos { get; set; }

    public virtual DbSet<Cita> Cita { get; set; }

    public virtual DbSet<Consultorio> Consultorios { get; set; }

    public virtual DbSet<Especialidad> Especialidads { get; set; }

    public virtual DbSet<HistorialMedico> HistorialMedicos { get; set; }

    public virtual DbSet<HorarioTrabajo> HorarioTrabajos { get; set; }

    public virtual DbSet<Hospital> Hospitals { get; set; }

    public virtual DbSet<MedicamentoRecetado> MedicamentoRecetados { get; set; }

    public virtual DbSet<Medico> Medicos { get; set; }

    public virtual DbSet<MedicoEspecialidadHospital> MedicoEspecialidadHospitals { get; set; }

    public virtual DbSet<Paciente> Pacientes { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<RecetaMedica> RecetaMedicas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//         => optionsBuilder.UseSqlServer("Server=localhost;Database=Citas;User Id=sa;Password=Test-123456;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cargo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_cargo");

            entity.ToTable("cargo");

            entity.HasIndex(e => e.EspecialidadId, "ix_cargo_especialidad_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.CreadoPor).HasColumnName("creado_por");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .HasColumnName("descripcion");
            entity.Property(e => e.EsJefatura).HasColumnName("es_jefatura");
            entity.Property(e => e.EspecialidadId).HasColumnName("especialidad_id");
            entity.Property(e => e.FechaActualizacion).HasColumnName("fecha_actualizacion");
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
            entity.Property(e => e.ModificadoPor).HasColumnName("modificado_por");
            entity.Property(e => e.NivelJerarquico).HasColumnName("nivel_jerarquico");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");

            // entity.HasOne(d => d.Especialidad).WithMany(p => p.Cargos)
            //     .HasForeignKey(d => d.EspecialidadId)
            //     .HasConstraintName("fk_cargo_especialidad_especialidad_id");
        });

        modelBuilder.Entity<Cita>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_cita");

            entity.ToTable("cita");

            entity.HasIndex(e => e.ConsultorioId, "ix_cita_consultorio_id");

            entity.HasIndex(e => e.Estado, "ix_cita_estado");

            entity.HasIndex(e => e.FechaHora, "ix_cita_fecha_hora");

            entity.HasIndex(e => e.MedicoId, "ix_cita_medico_id");

            entity.HasIndex(e => e.PacienteId, "ix_cita_paciente_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.ConsultorioId).HasColumnName("consultorio_id");
            entity.Property(e => e.CreadoPor).HasColumnName("creado_por");
            entity.Property(e => e.Diagnostico).HasColumnName("diagnostico");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasColumnName("estado");
            entity.Property(e => e.FechaActualizacion).HasColumnName("fecha_actualizacion");
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaHora).HasColumnName("fecha_hora");
            entity.Property(e => e.MedicoId).HasColumnName("medico_id");
            entity.Property(e => e.ModificadoPor).HasColumnName("modificado_por");
            entity.Property(e => e.Motivo)
                .HasMaxLength(200)
                .HasColumnName("motivo");
            entity.Property(e => e.Notas).HasColumnName("notas");
            entity.Property(e => e.PacienteId).HasColumnName("paciente_id");
            entity.Property(e => e.Tratamiento).HasColumnName("tratamiento");

            entity.HasOne(d => d.Consultorio).WithMany(p => p.Cita)
                .HasForeignKey(d => d.ConsultorioId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_cita_consultorio_consultorio_id");

            entity.HasOne(d => d.Medico).WithMany(p => p.Cita)
                .HasForeignKey(d => d.MedicoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_cita_medico_medico_id");

            entity.HasOne(d => d.Paciente).WithMany(p => p.Cita)
                .HasForeignKey(d => d.PacienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_cita_paciente_paciente_id");
        });

        modelBuilder.Entity<Consultorio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_consultorio");

            entity.ToTable("consultorio");

            entity.HasIndex(e => e.HospitalId, "ix_consultorio_hospital_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.CreadoPor).HasColumnName("creado_por");
            entity.Property(e => e.Equipamiento).HasColumnName("equipamiento");
            entity.Property(e => e.FechaActualizacion).HasColumnName("fecha_actualizacion");
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
            entity.Property(e => e.HospitalId).HasColumnName("hospital_id");
            entity.Property(e => e.ModificadoPor).HasColumnName("modificado_por");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.NumeroConsultorio)
                .HasMaxLength(20)
                .HasColumnName("numero_consultorio");
            entity.Property(e => e.Telefono).HasColumnName("telefono");
            entity.Property(e => e.Ubicacion).HasColumnName("ubicacion");

            entity.HasOne(d => d.Hospital).WithMany(p => p.Consultorios)
                .HasForeignKey(d => d.HospitalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_consultorio_hospital_hospital_id");
        });

        modelBuilder.Entity<Especialidad>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_especialidad");

            entity.ToTable("especialidad");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.CreadoPor).HasColumnName("creado_por");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.FechaActualizacion).HasColumnName("fecha_actualizacion");
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
            entity.Property(e => e.ModificadoPor).HasColumnName("modificado_por");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<HistorialMedico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_historial_medico");

            entity.ToTable("historial_medico");

            entity.HasIndex(e => e.CitaId, "ix_historial_medico_cita_id");

            entity.HasIndex(e => e.MedicoId, "ix_historial_medico_medico_id");

            entity.HasIndex(e => e.PacienteId, "ix_historial_medico_paciente_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.Altura)
                .HasColumnType("decimal(4, 2)")
                .HasColumnName("altura");
            entity.Property(e => e.CitaId).HasColumnName("cita_id");
            entity.Property(e => e.CreadoPor).HasColumnName("creado_por");
            entity.Property(e => e.Diagnostico).HasColumnName("diagnostico");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.FechaActualizacion).HasColumnName("fecha_actualizacion");
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
            entity.Property(e => e.MedicoId).HasColumnName("medico_id");
            entity.Property(e => e.ModificadoPor).HasColumnName("modificado_por");
            entity.Property(e => e.Notas).HasColumnName("notas");
            entity.Property(e => e.PacienteId).HasColumnName("paciente_id");
            entity.Property(e => e.Peso)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("peso");
            entity.Property(e => e.PresionArterial).HasColumnName("presion_arterial");
            entity.Property(e => e.Temperatura)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("temperatura");
            entity.Property(e => e.Tratamiento).HasColumnName("tratamiento");

            entity.HasOne(d => d.Cita).WithMany(p => p.HistorialMedicos)
                .HasForeignKey(d => d.CitaId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_historial_medico_cita_cita_id");

            entity.HasOne(d => d.Medico).WithMany(p => p.HistorialMedicos)
                .HasForeignKey(d => d.MedicoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_historial_medico_medico_medico_id");

            entity.HasOne(d => d.Paciente).WithMany(p => p.HistorialMedicos)
                .HasForeignKey(d => d.PacienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_historial_medico_paciente_paciente_id");
        });

        modelBuilder.Entity<HorarioTrabajo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_horario_trabajo");

            entity.ToTable("horario_trabajo");

            entity.HasIndex(e => e.MedicoId, "ix_horario_trabajo_medico_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.CreadoPor).HasColumnName("creado_por");
            entity.Property(e => e.DiaSemana).HasColumnName("dia_semana");
            entity.Property(e => e.FechaActualizacion).HasColumnName("fecha_actualizacion");
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
            entity.Property(e => e.HoraFin).HasColumnName("hora_fin");
            entity.Property(e => e.HoraInicio).HasColumnName("hora_inicio");
            entity.Property(e => e.MedicoId).HasColumnName("medico_id");
            entity.Property(e => e.ModificadoPor).HasColumnName("modificado_por");

            entity.HasOne(d => d.Medico).WithMany(p => p.HorarioTrabajos)
                .HasForeignKey(d => d.MedicoId)
                .HasConstraintName("fk_horario_trabajo_medico_medico_id");
        });

        modelBuilder.Entity<Hospital>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_hospital");

            entity.ToTable("hospital");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.CodigoPostal)
                .HasMaxLength(50)
                .HasColumnName("codigo_postal");
            entity.Property(e => e.CreadoPor).HasColumnName("creado_por");
            entity.Property(e => e.Direccion).HasColumnName("direccion");
            entity.Property(e => e.EmailContacto).HasColumnName("email_contacto");
            entity.Property(e => e.FechaActualizacion).HasColumnName("fecha_actualizacion");
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
            entity.Property(e => e.HorarioAtencion).HasColumnName("horario_atencion");
            entity.Property(e => e.ModificadoPor).HasColumnName("modificado_por");
            entity.Property(e => e.Municipio).HasColumnName("municipio");
            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .HasColumnName("nombre");
            entity.Property(e => e.Pais).HasColumnName("pais");
            entity.Property(e => e.ServiciosEspeciales).HasColumnName("servicios_especiales");
            entity.Property(e => e.SitioWeb).HasColumnName("sitio_web");
            entity.Property(e => e.TelefonoPrincipal).HasColumnName("telefono_principal");
            entity.Property(e => e.Url).HasColumnName("url");
        });

        modelBuilder.Entity<MedicamentoRecetado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_medicamento_recetado");

            entity.ToTable("medicamento_recetado");

            entity.HasIndex(e => e.RecetaMedicaId, "ix_medicamento_recetado_receta_medica_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.CreadoPor).HasColumnName("creado_por");
            entity.Property(e => e.Dosis).HasColumnName("dosis");
            entity.Property(e => e.FechaActualizacion).HasColumnName("fecha_actualizacion");
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
            entity.Property(e => e.Frecuencia).HasColumnName("frecuencia");
            entity.Property(e => e.InstruccionesEspeciales).HasColumnName("instrucciones_especiales");
            entity.Property(e => e.ModificadoPor).HasColumnName("modificado_por");
            entity.Property(e => e.NombreMedicamento)
                .HasMaxLength(100)
                .HasColumnName("nombre_medicamento");
            entity.Property(e => e.RecetaMedicaId).HasColumnName("receta_medica_id");

            entity.HasOne(d => d.RecetaMedica).WithMany(p => p.MedicamentoRecetados)
                .HasForeignKey(d => d.RecetaMedicaId)
                .HasConstraintName("fk_medicamento_recetado_receta_medica_receta_medica_id");
        });

        modelBuilder.Entity<Medico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_medico");

            entity.ToTable("medico");

    

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.Biografia).HasColumnName("biografia");
            entity.Property(e => e.CedulaProfesional).HasColumnName("cedula_profesional");
            entity.Property(e => e.CreadoPor).HasColumnName("creado_por");
            //entity.Property(e => e.EspecialidadId).HasColumnName("especialidad_id");
            entity.Property(e => e.FechaActualizacion).HasColumnName("fecha_actualizacion");
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
            //entity.Property(e => e.HospitalId).HasColumnName("hospital_id");
            entity.Property(e => e.ModificadoPor).HasColumnName("modificado_por");


        });






        modelBuilder.Entity<MedicoEspecialidadHospital>(entity =>
        {
            
            entity.HasKey(e => e.Id).HasName("pk_medico_especialidad_hospital");
            entity.ToTable("medico_especialidad_hospital"); 
            entity.HasIndex(e => e.CargoId, "ix_medico_especialidad_hospital_cargo_id");
            entity.HasIndex(e => e.EspecialidadId, "ix_medico_especialidad_hospital_especialidad_id");
            entity.HasIndex(e => e.HospitalId, "ix_medico_especialidad_hospital_hospital_id");
            entity.HasIndex(e => new { e.MedicoId, e.EspecialidadId, e.HospitalId }, "ix_medico_especialidad_hospital_medico_id_especialidad_id_hospital_id").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.CargoId).HasColumnName("cargo_id");
            entity.Property(e => e.Consultorio).HasColumnName("consultorio");
            entity.Property(e => e.CostoConsultaEspecifico)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("costo_consulta_especifico");
            entity.Property(e => e.CreadoPor).HasColumnName("creado_por");
            entity.Property(e => e.EspecialidadId).HasColumnName("especialidad_id");
            entity.Property(e => e.FechaActualizacion).HasColumnName("fecha_actualizacion");
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaFin).HasColumnName("fecha_fin");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
            entity.Property(e => e.HorarioAtencion).HasColumnName("horario_atencion");
            entity.Property(e => e.HospitalId).HasColumnName("hospital_id");
            entity.Property(e => e.MedicoId).HasColumnName("medico_id");
            entity.Property(e => e.ModificadoPor).HasColumnName("modificado_por");
            entity.Property(e => e.NumeroContrato).HasColumnName("numero_contrato");
            entity.Property(e => e.TipoContratacion).HasColumnName("tipo_contratacion");

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
                
            // Índice único para evitar duplicados activos
            entity.HasIndex(e => new { e.MedicoId, e.EspecialidadId, e.HospitalId }, "IX_MedicoEspecialidadHospital_Unique")
            .IsUnique()
            .HasFilter("[Activo] = 1");
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_paciente");

            entity.ToTable("paciente");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.Alergias).HasColumnName("alergias");
            entity.Property(e => e.CreadoPor).HasColumnName("creado_por");
            entity.Property(e => e.EnfermedadesCronicas).HasColumnName("enfermedades_cronicas");
            entity.Property(e => e.FechaActualizacion).HasColumnName("fecha_actualizacion");
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaNacimiento).HasColumnName("fecha_nacimiento");
            entity.Property(e => e.Genero)
                .HasMaxLength(20)
                .HasColumnName("genero");
            entity.Property(e => e.MedicamentosActuales).HasColumnName("medicamentos_actuales");
            entity.Property(e => e.ModificadoPor).HasColumnName("modificado_por");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_pago");

            entity.ToTable("pago");

            entity.HasIndex(e => e.CitaId, "ix_pago_cita_id")
                .IsUnique()
                .HasFilter("([cita_id] IS NOT NULL)");

            entity.HasIndex(e => e.Estado, "ix_pago_estado");

            entity.HasIndex(e => e.PacienteId, "ix_pago_paciente_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.CitaId).HasColumnName("cita_id");
            entity.Property(e => e.Comprobante).HasColumnName("comprobante");
            entity.Property(e => e.CreadoPor).HasColumnName("creado_por");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasColumnName("estado");
            entity.Property(e => e.FechaActualizacion).HasColumnName("fecha_actualizacion");
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaPago).HasColumnName("fecha_pago");
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(20)
                .HasColumnName("metodo_pago");
            entity.Property(e => e.ModificadoPor).HasColumnName("modificado_por");
            entity.Property(e => e.Monto)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("monto");
            entity.Property(e => e.Notas).HasColumnName("notas");
            entity.Property(e => e.PacienteId).HasColumnName("paciente_id");

            entity.HasOne(d => d.Cita).WithOne(p => p.Pago)
                .HasForeignKey<Pago>(d => d.CitaId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_pago_cita_cita_id");

            entity.HasOne(d => d.Paciente).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.PacienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_pago_paciente_paciente_id");
        });

        modelBuilder.Entity<RecetaMedica>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_receta_medica");

            entity.ToTable("receta_medica");

            entity.HasIndex(e => e.CitaId, "ix_receta_medica_cita_id");

            entity.HasIndex(e => e.FechaEmision, "ix_receta_medica_fecha_emision");

            entity.HasIndex(e => e.MedicoId, "ix_receta_medica_medico_id");

            entity.HasIndex(e => e.PacienteId, "ix_receta_medica_paciente_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.CitaId).HasColumnName("cita_id");
            entity.Property(e => e.CreadoPor).HasColumnName("creado_por");
            entity.Property(e => e.FechaActualizacion).HasColumnName("fecha_actualizacion");
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaEmision).HasColumnName("fecha_emision");
            entity.Property(e => e.FechaVencimiento).HasColumnName("fecha_vencimiento");
            entity.Property(e => e.Instrucciones).HasColumnName("instrucciones");
            entity.Property(e => e.MedicoId).HasColumnName("medico_id");
            entity.Property(e => e.ModificadoPor).HasColumnName("modificado_por");
            entity.Property(e => e.PacienteId).HasColumnName("paciente_id");

            entity.HasOne(d => d.Cita).WithMany(p => p.RecetaMedicas)
                .HasForeignKey(d => d.CitaId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_receta_medica_cita_cita_id");

            entity.HasOne(d => d.Medico).WithMany(p => p.RecetaMedicas)
                .HasForeignKey(d => d.MedicoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_receta_medica_medico_medico_id");

            entity.HasOne(d => d.Paciente).WithMany(p => p.RecetaMedicas)
                .HasForeignKey(d => d.PacienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_receta_medica_paciente_paciente_id");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_usuario");

            entity.ToTable("usuario");

            entity.HasIndex(e => e.IdentityId, "ix_usuario_identity_id").IsUnique();

            entity.HasIndex(e => e.MedicoId, "ix_usuario_medico_id")
                .IsUnique()
                .HasFilter("([medico_id] IS NOT NULL)");

            entity.HasIndex(e => e.PacienteId, "ix_usuario_paciente_id")
                .IsUnique()
                .HasFilter("([paciente_id] IS NOT NULL)");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.Apellido).HasColumnName("apellido");
            entity.Property(e => e.Celular).HasColumnName("celular");
            entity.Property(e => e.Ciudad).HasColumnName("ciudad");
            entity.Property(e => e.CodigoPais).HasColumnName("codigo_pais");
            entity.Property(e => e.CreadoPor).HasColumnName("creado_por");
            entity.Property(e => e.Direccion).HasColumnName("direccion");
            entity.Property(e => e.Email)
                .HasDefaultValue("")
                .HasColumnName("email");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaActualizacion).HasColumnName("fecha_actualizacion");
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
            entity.Property(e => e.IdentityId).HasColumnName("identity_id");
            entity.Property(e => e.MedicoId).HasColumnName("medico_id");
            entity.Property(e => e.ModificadoPor).HasColumnName("modificado_por");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
            entity.Property(e => e.PacienteId).HasColumnName("paciente_id");
            entity.Property(e => e.Pais).HasColumnName("pais");
            entity.Property(e => e.RolId).HasColumnName("rol_id");
            entity.Property(e => e.RolName).HasColumnName("rol_name");
            entity.Property(e => e.Telefono).HasColumnName("telefono");
            entity.Property(e => e.UltimoLogin).HasColumnName("ultimo_login");

            entity.HasOne(d => d.Medico).WithOne(p => p.Usuario)
                .HasForeignKey<Usuario>(d => d.MedicoId)
                .HasConstraintName("fk_usuario_medico_medico_id");

            entity.HasOne(d => d.Paciente).WithOne(p => p.Usuario)
                .HasForeignKey<Usuario>(d => d.PacienteId)
                .HasConstraintName("fk_usuario_paciente_paciente_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
