using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppCapasCitas.API.Migrations
{
    /// <inheritdoc />
    public partial class QuitarRelacionCargoEspecialidad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_cargo_especialidad_especialidad_id",
                table: "cargo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "fk_cargo_especialidad_especialidad_id",
                table: "cargo",
                column: "especialidad_id",
                principalTable: "especialidad",
                principalColumn: "id");
        }
    }
}
