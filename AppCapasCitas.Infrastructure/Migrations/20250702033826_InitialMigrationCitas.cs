using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppCapasCitas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrationCitas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "especialidad",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    creado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_especialidad", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hospital",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    telefono_principal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email_contacto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sitio_web = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    direccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    codigo_postal = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    municipio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pais = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    horario_atencion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    servicios_especiales = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    creado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hospital", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tipo_accion",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tipo_accion", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    role_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    rol_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    celular = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    direccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ciudad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    codigo_pais = table.Column<int>(type: "int", nullable: false),
                    pais = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ultimo_login = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    creado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usuario", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cargo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    nivel_jerarquico = table.Column<int>(type: "int", nullable: false),
                    es_jefatura = table.Column<bool>(type: "bit", nullable: false),
                    especialidad_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    creado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cargo", x => x.id);
                    table.ForeignKey(
                        name: "fk_cargo_especialidad_especialidad_id",
                        column: x => x.especialidad_id,
                        principalTable: "especialidad",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "consultorio",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ubicacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    numero_consultorio = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    equipamiento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    hospital_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    creado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_consultorio", x => x.id);
                    table.ForeignKey(
                        name: "fk_consultorio_hospital_hospital_id",
                        column: x => x.hospital_id,
                        principalTable: "hospital",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "medico",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    cedula_profesional = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    biografia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    creado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_medico", x => x.id);
                    table.ForeignKey(
                        name: "fk_medico_usuario_id",
                        column: x => x.id,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "paciente",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fecha_nacimiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    genero = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    alergias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    enfermedades_cronicas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    medicamentos_actuales = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    creado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_paciente", x => x.id);
                    table.ForeignKey(
                        name: "fk_paciente_usuario_id",
                        column: x => x.id,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "usuario_log_accion",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    usuario_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    tipo_accion_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    creado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usuario_log_accion", x => x.id);
                    table.ForeignKey(
                        name: "fk_usuario_log_accion_tipo_accion_tipo_accion_id",
                        column: x => x.tipo_accion_id,
                        principalTable: "tipo_accion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_usuario_log_accion_usuario_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "horario_trabajo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    dia_semana = table.Column<int>(type: "int", nullable: false),
                    hora_inicio = table.Column<TimeSpan>(type: "time", nullable: false),
                    hora_fin = table.Column<TimeSpan>(type: "time", nullable: false),
                    medico_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    creado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_horario_trabajo", x => x.id);
                    table.ForeignKey(
                        name: "fk_horario_trabajo_medico_medico_id",
                        column: x => x.medico_id,
                        principalTable: "medico",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "medico_especialidad_hospital",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fecha_inicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_fin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    costo_consulta_especifico = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    consultorio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    horario_atencion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    numero_contrato = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tipo_contratacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    medico_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    especialidad_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    hospital_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    cargo_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    creado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_medico_especialidad_hospital", x => x.id);
                    table.ForeignKey(
                        name: "fk_medico_especialidad_hospital_cargo_cargo_id",
                        column: x => x.cargo_id,
                        principalTable: "cargo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_medico_especialidad_hospital_especialidad_especialidad_id",
                        column: x => x.especialidad_id,
                        principalTable: "especialidad",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_medico_especialidad_hospital_hospital_hospital_id",
                        column: x => x.hospital_id,
                        principalTable: "hospital",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_medico_especialidad_hospital_medico_medico_id",
                        column: x => x.medico_id,
                        principalTable: "medico",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cita",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fecha_hora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    motivo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    notas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    diagnostico = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tratamiento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    paciente_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    medico_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    consultorio_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    pago_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    creado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cita", x => x.id);
                    table.ForeignKey(
                        name: "fk_cita_consultorio_consultorio_id",
                        column: x => x.consultorio_id,
                        principalTable: "consultorio",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_cita_medico_medico_id",
                        column: x => x.medico_id,
                        principalTable: "medico",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_cita_paciente_paciente_id",
                        column: x => x.paciente_id,
                        principalTable: "paciente",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "historial_medico",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    diagnostico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tratamiento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    notas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    presion_arterial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    temperatura = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    peso = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    altura = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    paciente_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    medico_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    cita_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    creado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_historial_medico", x => x.id);
                    table.ForeignKey(
                        name: "fk_historial_medico_cita_cita_id",
                        column: x => x.cita_id,
                        principalTable: "cita",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_historial_medico_medico_medico_id",
                        column: x => x.medico_id,
                        principalTable: "medico",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_historial_medico_paciente_paciente_id",
                        column: x => x.paciente_id,
                        principalTable: "paciente",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "pago",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    monto = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    fecha_pago = table.Column<DateTime>(type: "datetime2", nullable: false),
                    metodo_pago = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    comprobante = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    notas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    paciente_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    cita_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    creado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pago", x => x.id);
                    table.ForeignKey(
                        name: "fk_pago_cita_cita_id",
                        column: x => x.cita_id,
                        principalTable: "cita",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_pago_paciente_paciente_id",
                        column: x => x.paciente_id,
                        principalTable: "paciente",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "receta_medica",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fecha_emision = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_vencimiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    instrucciones = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    medico_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    paciente_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    cita_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    creado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_receta_medica", x => x.id);
                    table.ForeignKey(
                        name: "fk_receta_medica_cita_cita_id",
                        column: x => x.cita_id,
                        principalTable: "cita",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_receta_medica_medico_medico_id",
                        column: x => x.medico_id,
                        principalTable: "medico",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_receta_medica_paciente_paciente_id",
                        column: x => x.paciente_id,
                        principalTable: "paciente",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "medicamento_recetado",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nombre_medicamento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    dosis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    frecuencia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    instrucciones_especiales = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    receta_medica_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    creado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_medicamento_recetado", x => x.id);
                    table.ForeignKey(
                        name: "fk_medicamento_recetado_receta_medica_receta_medica_id",
                        column: x => x.receta_medica_id,
                        principalTable: "receta_medica",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_cargo_especialidad_id",
                table: "cargo",
                column: "especialidad_id");

            migrationBuilder.CreateIndex(
                name: "ix_cita_consultorio_id",
                table: "cita",
                column: "consultorio_id");

            migrationBuilder.CreateIndex(
                name: "ix_cita_estado",
                table: "cita",
                column: "estado");

            migrationBuilder.CreateIndex(
                name: "ix_cita_fecha_hora",
                table: "cita",
                column: "fecha_hora");

            migrationBuilder.CreateIndex(
                name: "ix_cita_medico_id",
                table: "cita",
                column: "medico_id");

            migrationBuilder.CreateIndex(
                name: "ix_cita_paciente_id",
                table: "cita",
                column: "paciente_id");

            migrationBuilder.CreateIndex(
                name: "ix_consultorio_hospital_id",
                table: "consultorio",
                column: "hospital_id");

            migrationBuilder.CreateIndex(
                name: "ix_historial_medico_cita_id",
                table: "historial_medico",
                column: "cita_id");

            migrationBuilder.CreateIndex(
                name: "ix_historial_medico_medico_id",
                table: "historial_medico",
                column: "medico_id");

            migrationBuilder.CreateIndex(
                name: "ix_historial_medico_paciente_id",
                table: "historial_medico",
                column: "paciente_id");

            migrationBuilder.CreateIndex(
                name: "ix_horario_trabajo_medico_id",
                table: "horario_trabajo",
                column: "medico_id");

            migrationBuilder.CreateIndex(
                name: "ix_medicamento_recetado_receta_medica_id",
                table: "medicamento_recetado",
                column: "receta_medica_id");

            migrationBuilder.CreateIndex(
                name: "ix_medico_especialidad_hospital_cargo_id",
                table: "medico_especialidad_hospital",
                column: "cargo_id");

            migrationBuilder.CreateIndex(
                name: "ix_medico_especialidad_hospital_especialidad_id",
                table: "medico_especialidad_hospital",
                column: "especialidad_id");

            migrationBuilder.CreateIndex(
                name: "ix_medico_especialidad_hospital_hospital_id",
                table: "medico_especialidad_hospital",
                column: "hospital_id");

            migrationBuilder.CreateIndex(
                name: "ix_medico_especialidad_hospital_medico_id_especialidad_id_hospital_id",
                table: "medico_especialidad_hospital",
                columns: new[] { "medico_id", "especialidad_id", "hospital_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_pago_cita_id",
                table: "pago",
                column: "cita_id",
                unique: true,
                filter: "[cita_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_pago_estado",
                table: "pago",
                column: "estado");

            migrationBuilder.CreateIndex(
                name: "ix_pago_paciente_id",
                table: "pago",
                column: "paciente_id");

            migrationBuilder.CreateIndex(
                name: "ix_receta_medica_cita_id",
                table: "receta_medica",
                column: "cita_id");

            migrationBuilder.CreateIndex(
                name: "ix_receta_medica_fecha_emision",
                table: "receta_medica",
                column: "fecha_emision");

            migrationBuilder.CreateIndex(
                name: "ix_receta_medica_medico_id",
                table: "receta_medica",
                column: "medico_id");

            migrationBuilder.CreateIndex(
                name: "ix_receta_medica_paciente_id",
                table: "receta_medica",
                column: "paciente_id");

            migrationBuilder.CreateIndex(
                name: "ix_usuario_id",
                table: "usuario",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_usuario_log_accion_tipo_accion_id",
                table: "usuario_log_accion",
                column: "tipo_accion_id");

            migrationBuilder.CreateIndex(
                name: "ix_usuario_log_accion_usuario_id",
                table: "usuario_log_accion",
                column: "usuario_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "historial_medico");

            migrationBuilder.DropTable(
                name: "horario_trabajo");

            migrationBuilder.DropTable(
                name: "medicamento_recetado");

            migrationBuilder.DropTable(
                name: "medico_especialidad_hospital");

            migrationBuilder.DropTable(
                name: "pago");

            migrationBuilder.DropTable(
                name: "usuario_log_accion");

            migrationBuilder.DropTable(
                name: "receta_medica");

            migrationBuilder.DropTable(
                name: "cargo");

            migrationBuilder.DropTable(
                name: "tipo_accion");

            migrationBuilder.DropTable(
                name: "cita");

            migrationBuilder.DropTable(
                name: "especialidad");

            migrationBuilder.DropTable(
                name: "consultorio");

            migrationBuilder.DropTable(
                name: "medico");

            migrationBuilder.DropTable(
                name: "paciente");

            migrationBuilder.DropTable(
                name: "hospital");

            migrationBuilder.DropTable(
                name: "usuario");
        }
    }
}
