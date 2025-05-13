using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppCapasCitas.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrationCitasApp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "especialidad",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                name: "paciente",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fecha_nacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    genero = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "cargo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    nivel_jerarquico = table.Column<int>(type: "int", nullable: false),
                    es_jefatura = table.Column<bool>(type: "bit", nullable: false),
                    especialidad_id = table.Column<int>(type: "int", nullable: true),
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
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ubicacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    numero_consultorio = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    equipamiento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    hospital_id = table.Column<int>(type: "int", nullable: false),
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
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cedula_profesional = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    biografia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    hospital_id = table.Column<int>(type: "int", nullable: true),
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
                        name: "fk_medico_hospital_hospital_id",
                        column: x => x.hospital_id,
                        principalTable: "hospital",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "citas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fecha_hora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    motivo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    notas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    diagnostico = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tratamiento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    paciente_id = table.Column<int>(type: "int", nullable: false),
                    medico_id = table.Column<int>(type: "int", nullable: false),
                    consultorio_id = table.Column<int>(type: "int", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    creado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_citas", x => x.id);
                    table.ForeignKey(
                        name: "fk_citas_consultorio_consultorio_id",
                        column: x => x.consultorio_id,
                        principalTable: "consultorio",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_citas_medico_medico_id",
                        column: x => x.medico_id,
                        principalTable: "medico",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_citas_paciente_paciente_id",
                        column: x => x.paciente_id,
                        principalTable: "paciente",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "horarios_trabajo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dia_semana = table.Column<int>(type: "int", nullable: false),
                    hora_inicio = table.Column<TimeSpan>(type: "time", nullable: false),
                    hora_fin = table.Column<TimeSpan>(type: "time", nullable: false),
                    medico_id = table.Column<int>(type: "int", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    creado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_horarios_trabajo", x => x.id);
                    table.ForeignKey(
                        name: "fk_horarios_trabajo_medico_medico_id",
                        column: x => x.medico_id,
                        principalTable: "medico",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "medico_especialidad_hospital",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fecha_inicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_fin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    costo_consulta_especifico = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    consultorio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    horario_atencion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    numero_contrato = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tipo_contratacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    medico_id = table.Column<int>(type: "int", nullable: false),
                    especialidad_id = table.Column<int>(type: "int", nullable: false),
                    hospital_id = table.Column<int>(type: "int", nullable: false),
                    cargo_id = table.Column<int>(type: "int", nullable: false),
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
                name: "usuario",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    identity_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    rol_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    paciente_id = table.Column<int>(type: "int", nullable: true),
                    medico_id = table.Column<int>(type: "int", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    creado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usuario", x => x.id);
                    table.ForeignKey(
                        name: "fk_usuario_medico_medico_id",
                        column: x => x.medico_id,
                        principalTable: "medico",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_usuario_paciente_paciente_id",
                        column: x => x.paciente_id,
                        principalTable: "paciente",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "historial_medico",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    diagnostico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tratamiento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    notas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    presion_arterial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    temperatura = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    peso = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    altura = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    paciente_id = table.Column<int>(type: "int", nullable: false),
                    medico_id = table.Column<int>(type: "int", nullable: false),
                    cita_id = table.Column<int>(type: "int", nullable: true),
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
                        name: "fk_historial_medico_citas_cita_id",
                        column: x => x.cita_id,
                        principalTable: "citas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
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
                name: "pagos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    monto = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    fecha_pago = table.Column<DateTime>(type: "datetime2", nullable: false),
                    metodo_pago = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    comprobante = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    notas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    paciente_id = table.Column<int>(type: "int", nullable: false),
                    cita_id = table.Column<int>(type: "int", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    creado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pagos", x => x.id);
                    table.ForeignKey(
                        name: "fk_pagos_citas_cita_id",
                        column: x => x.cita_id,
                        principalTable: "citas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_pagos_paciente_paciente_id",
                        column: x => x.paciente_id,
                        principalTable: "paciente",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "recetas_medicas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fecha_emision = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_vencimiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    instrucciones = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    medico_id = table.Column<int>(type: "int", nullable: false),
                    paciente_id = table.Column<int>(type: "int", nullable: false),
                    cita_id = table.Column<int>(type: "int", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    creado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_recetas_medicas", x => x.id);
                    table.ForeignKey(
                        name: "fk_recetas_medicas_citas_cita_id",
                        column: x => x.cita_id,
                        principalTable: "citas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_recetas_medicas_medico_medico_id",
                        column: x => x.medico_id,
                        principalTable: "medico",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_recetas_medicas_paciente_paciente_id",
                        column: x => x.paciente_id,
                        principalTable: "paciente",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "medicamentos_recetados",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_medicamento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    dosis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    frecuencia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    instrucciones_especiales = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    receta_medica_id = table.Column<int>(type: "int", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    creado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_medicamentos_recetados", x => x.id);
                    table.ForeignKey(
                        name: "fk_medicamentos_recetados_recetas_medicas_receta_medica_id",
                        column: x => x.receta_medica_id,
                        principalTable: "recetas_medicas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_cargo_especialidad_id",
                table: "cargo",
                column: "especialidad_id");

            migrationBuilder.CreateIndex(
                name: "ix_citas_consultorio_id",
                table: "citas",
                column: "consultorio_id");

            migrationBuilder.CreateIndex(
                name: "ix_citas_estado",
                table: "citas",
                column: "estado");

            migrationBuilder.CreateIndex(
                name: "ix_citas_fecha_hora",
                table: "citas",
                column: "fecha_hora");

            migrationBuilder.CreateIndex(
                name: "ix_citas_medico_id",
                table: "citas",
                column: "medico_id");

            migrationBuilder.CreateIndex(
                name: "ix_citas_paciente_id",
                table: "citas",
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
                name: "ix_horarios_trabajo_medico_id",
                table: "horarios_trabajo",
                column: "medico_id");

            migrationBuilder.CreateIndex(
                name: "ix_medicamentos_recetados_receta_medica_id",
                table: "medicamentos_recetados",
                column: "receta_medica_id");

            migrationBuilder.CreateIndex(
                name: "ix_medico_hospital_id",
                table: "medico",
                column: "hospital_id");

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
                name: "ix_pagos_cita_id",
                table: "pagos",
                column: "cita_id",
                unique: true,
                filter: "[cita_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_pagos_estado",
                table: "pagos",
                column: "estado");

            migrationBuilder.CreateIndex(
                name: "ix_pagos_paciente_id",
                table: "pagos",
                column: "paciente_id");

            migrationBuilder.CreateIndex(
                name: "ix_recetas_medicas_cita_id",
                table: "recetas_medicas",
                column: "cita_id");

            migrationBuilder.CreateIndex(
                name: "ix_recetas_medicas_fecha_emision",
                table: "recetas_medicas",
                column: "fecha_emision");

            migrationBuilder.CreateIndex(
                name: "ix_recetas_medicas_medico_id",
                table: "recetas_medicas",
                column: "medico_id");

            migrationBuilder.CreateIndex(
                name: "ix_recetas_medicas_paciente_id",
                table: "recetas_medicas",
                column: "paciente_id");

            migrationBuilder.CreateIndex(
                name: "ix_usuario_identity_id",
                table: "usuario",
                column: "identity_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_usuario_medico_id",
                table: "usuario",
                column: "medico_id",
                unique: true,
                filter: "[medico_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_usuario_paciente_id",
                table: "usuario",
                column: "paciente_id",
                unique: true,
                filter: "[paciente_id] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "historial_medico");

            migrationBuilder.DropTable(
                name: "horarios_trabajo");

            migrationBuilder.DropTable(
                name: "medicamentos_recetados");

            migrationBuilder.DropTable(
                name: "medico_especialidad_hospital");

            migrationBuilder.DropTable(
                name: "pagos");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "recetas_medicas");

            migrationBuilder.DropTable(
                name: "cargo");

            migrationBuilder.DropTable(
                name: "citas");

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
        }
    }
}
