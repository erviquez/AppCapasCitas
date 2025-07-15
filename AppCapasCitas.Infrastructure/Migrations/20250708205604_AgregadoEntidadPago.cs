using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppCapasCitas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregadoEntidadPago : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "costo_consulta_base",
                table: "especialidad",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "costo_consulta",
                table: "cita",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "tipo_consulta_id",
                table: "cita",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TipoConsulta",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    multiplicador_costo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    duracion_minutos = table.Column<int>(type: "int", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    creado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modificado_por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tipo_consulta", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_cita_tipo_consulta_id",
                table: "cita",
                column: "tipo_consulta_id");

            migrationBuilder.AddForeignKey(
                name: "fk_cita_tipo_consulta_tipo_consulta_id",
                table: "cita",
                column: "tipo_consulta_id",
                principalTable: "TipoConsulta",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_cita_tipo_consulta_tipo_consulta_id",
                table: "cita");

            migrationBuilder.DropTable(
                name: "TipoConsulta");

            migrationBuilder.DropIndex(
                name: "ix_cita_tipo_consulta_id",
                table: "cita");

            migrationBuilder.DropColumn(
                name: "costo_consulta_base",
                table: "especialidad");

            migrationBuilder.DropColumn(
                name: "costo_consulta",
                table: "cita");

            migrationBuilder.DropColumn(
                name: "tipo_consulta_id",
                table: "cita");
        }
    }
}
