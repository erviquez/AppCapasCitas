using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppCapasCitas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModificandoEntidadMedicoEspecialidadHospital_y_TipoConsulta_nombre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "consultorio",
                table: "medico_especialidad_hospital");

            migrationBuilder.DropColumn(
                name: "horario_atencion",
                table: "medico_especialidad_hospital");

            migrationBuilder.DropColumn(
                name: "numero_contrato",
                table: "medico_especialidad_hospital");

            migrationBuilder.DropColumn(
                name: "tipo_contratacion",
                table: "medico_especialidad_hospital");

            migrationBuilder.RenameTable(
                name: "TipoConsulta",
                newName: "tipo_consulta");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "tipo_consulta",
                newName: "TipoConsulta");

            migrationBuilder.AddColumn<string>(
                name: "consultorio",
                table: "medico_especialidad_hospital",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "horario_atencion",
                table: "medico_especialidad_hospital",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "numero_contrato",
                table: "medico_especialidad_hospital",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tipo_contratacion",
                table: "medico_especialidad_hospital",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
