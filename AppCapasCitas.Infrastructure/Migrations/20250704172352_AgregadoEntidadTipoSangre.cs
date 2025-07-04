using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppCapasCitas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregadoEntidadTipoSangre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "tipo_sangre_id",
                table: "paciente",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tipo_sangre",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    antigenos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    anticuerpos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    recibir_de = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    donar_a = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tipo_sangre", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_paciente_tipo_sangre_id",
                table: "paciente",
                column: "tipo_sangre_id");

            migrationBuilder.CreateIndex(
                name: "ix_tipo_sangre_nombre",
                table: "tipo_sangre",
                column: "nombre",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_paciente_tipo_sangre_tipo_sangre_id",
                table: "paciente",
                column: "tipo_sangre_id",
                principalTable: "tipo_sangre",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_paciente_tipo_sangre_tipo_sangre_id",
                table: "paciente");

            migrationBuilder.DropTable(
                name: "tipo_sangre");

            migrationBuilder.DropIndex(
                name: "ix_paciente_tipo_sangre_id",
                table: "paciente");

            migrationBuilder.DropColumn(
                name: "tipo_sangre_id",
                table: "paciente");
        }
    }
}
