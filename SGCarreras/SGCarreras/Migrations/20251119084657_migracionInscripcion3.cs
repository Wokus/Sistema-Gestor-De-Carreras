using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGCarreras.Migrations
{
    /// <inheritdoc />
    public partial class migracionInscripcion3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inscripcion_Registro_RegistroId",
                table: "Inscripcion");

            migrationBuilder.AddForeignKey(
                name: "FK_Inscripcion_Registro_RegistroId",
                table: "Inscripcion",
                column: "RegistroId",
                principalTable: "Registro",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inscripcion_Registro_RegistroId",
                table: "Inscripcion");

            migrationBuilder.AddForeignKey(
                name: "FK_Inscripcion_Registro_RegistroId",
                table: "Inscripcion",
                column: "RegistroId",
                principalTable: "Registro",
                principalColumn: "Id");
        }
    }
}
