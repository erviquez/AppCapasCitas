using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppCapasCitas.Identity.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedFieldsControlsRolesDbContextIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Orden",
                table: "AspNetRoles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Prioridad",
                table: "AspNetRoles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activo",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "Orden",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "Prioridad",
                table: "AspNetRoles");
        }
    }
}
