using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppCapasCitas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditandoPacienteCamposAdicionales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ciudad",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "codigo_postal",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "idiomas",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "nacionalidad",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "numero_identificacion",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "sexo",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "tipo_sangre",
                table: "paciente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ciudad",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "codigo_postal",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "idiomas",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nacionalidad",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "numero_identificacion",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "sexo",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tipo_sangre",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
