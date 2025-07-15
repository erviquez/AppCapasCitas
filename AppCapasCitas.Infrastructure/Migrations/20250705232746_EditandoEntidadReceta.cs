using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppCapasCitas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditandoEntidadReceta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "adjuntos",
                table: "receta_medica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "diagnostico_principal",
                table: "receta_medica",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "diagnosticos_secundarios",
                table: "receta_medica",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "estado",
                table: "receta_medica",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "motivo",
                table: "receta_medica",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "observaciones",
                table: "receta_medica",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "universidad",
                table: "medico",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "adjuntos",
                table: "receta_medica");

            migrationBuilder.DropColumn(
                name: "diagnostico_principal",
                table: "receta_medica");

            migrationBuilder.DropColumn(
                name: "diagnosticos_secundarios",
                table: "receta_medica");

            migrationBuilder.DropColumn(
                name: "estado",
                table: "receta_medica");

            migrationBuilder.DropColumn(
                name: "motivo",
                table: "receta_medica");

            migrationBuilder.DropColumn(
                name: "observaciones",
                table: "receta_medica");

            migrationBuilder.DropColumn(
                name: "universidad",
                table: "medico");
        }
    }
}
