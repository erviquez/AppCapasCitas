using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppCapasCitas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregadoEntidadAseguradora : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ciudad_aseguradora",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "codigo_postal_aseguradora",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "direccion_aseguradora",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "estado_aseguradora",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "nombre_aseguradora",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "numero_seguro_medico",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "observaciones_adicionales",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "pais_aseguradora",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "seguro_medico",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "telefono_aseguradora",
                table: "paciente");

            migrationBuilder.CreateTable(
                name: "aseguradora",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ciudad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    codigo_postal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    estado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pais = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    paciente_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    creado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_aseguradora", x => x.id);
                    table.ForeignKey(
                        name: "fk_aseguradora_paciente_paciente_id",
                        column: x => x.paciente_id,
                        principalTable: "paciente",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_aseguradora_paciente_id",
                table: "aseguradora",
                column: "paciente_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "aseguradora");

            migrationBuilder.AddColumn<string>(
                name: "ciudad_aseguradora",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "codigo_postal_aseguradora",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "direccion_aseguradora",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "estado_aseguradora",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nombre_aseguradora",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "numero_seguro_medico",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "observaciones_adicionales",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "pais_aseguradora",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "seguro_medico",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "telefono_aseguradora",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
