using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppCapasCitas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregadoEntidadContactoPaciente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "contacto_emergencia_nombre",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "contacto_emergencia_parentesco",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "contacto_emergencia_telefono",
                table: "paciente");

            migrationBuilder.CreateTable(
                name: "contacto",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    telefono = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    parentesco = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    paciente_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contacto", x => x.id);
                    table.ForeignKey(
                        name: "fk_contacto_paciente_paciente_id",
                        column: x => x.paciente_id,
                        principalTable: "paciente",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_contacto_paciente_id",
                table: "contacto",
                column: "paciente_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contacto");

            migrationBuilder.AddColumn<string>(
                name: "contacto_emergencia_nombre",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "contacto_emergencia_parentesco",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "contacto_emergencia_telefono",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
