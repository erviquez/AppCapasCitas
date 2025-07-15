using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppCapasCitas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CompletandoEntidadTipoConsultaCit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_cita_tipo_consulta_tipo_consulta_id",
                table: "cita");

            migrationBuilder.AddForeignKey(
                name: "fk_cita_tipo_consulta_tipo_consulta_id",
                table: "cita",
                column: "tipo_consulta_id",
                principalTable: "TipoConsulta",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_cita_tipo_consulta_tipo_consulta_id",
                table: "cita");

            migrationBuilder.AddForeignKey(
                name: "fk_cita_tipo_consulta_tipo_consulta_id",
                table: "cita",
                column: "tipo_consulta_id",
                principalTable: "TipoConsulta",
                principalColumn: "id");
        }
    }
}
