using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGCarreras.Migrations
{
    /// <inheritdoc />
    public partial class SepararInscripcionDeRegistro3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "Inscripcion");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "Inscripcion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoriaId",
                table: "Inscripcion",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "Inscripcion",
                type: "TEXT",
                nullable: true);
        }
    }
}
