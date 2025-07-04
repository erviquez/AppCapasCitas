using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppCapasCitas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregadoDatosAdiconalesPaciente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "antecedentes_familiares",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "antecedentes_personales",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ciudad",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ciudad_aseguradora",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "codigo_postal",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "codigo_postal_aseguradora",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

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
                name: "estado_civil",
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
                name: "nombre_aseguradora",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "numero_identificacion",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "numero_seguro_medico",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "observaciones",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "observaciones_adicionales",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ocupacion",
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
                name: "sexo",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "telefono_aseguradora",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tipo_sangre",
                table: "paciente",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "antecedentes_familiares",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "antecedentes_personales",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "ciudad",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "ciudad_aseguradora",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "codigo_postal",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "codigo_postal_aseguradora",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "contacto_emergencia_nombre",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "contacto_emergencia_parentesco",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "contacto_emergencia_telefono",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "direccion_aseguradora",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "estado_aseguradora",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "estado_civil",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "idiomas",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "nacionalidad",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "nombre_aseguradora",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "numero_identificacion",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "numero_seguro_medico",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "observaciones",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "observaciones_adicionales",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "ocupacion",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "pais_aseguradora",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "seguro_medico",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "sexo",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "telefono_aseguradora",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "tipo_sangre",
                table: "paciente");
        }
    }
}
