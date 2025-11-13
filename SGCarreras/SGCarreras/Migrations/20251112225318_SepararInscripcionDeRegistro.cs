using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGCarreras.Migrations
{
    /// <inheritdoc />
    public partial class SepararInscripcionDeRegistro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inscripcion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FechaInscripcion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NumeroCorredor = table.Column<int>(type: "INTEGER", nullable: false),
                    Estado = table.Column<int>(type: "INTEGER", nullable: false),
                    Observaciones = table.Column<string>(type: "TEXT", nullable: true),
                    CorredorId = table.Column<int>(type: "INTEGER", nullable: false),
                    CarreraId = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoriaId = table.Column<int>(type: "INTEGER", nullable: false),
                    RegistroId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inscripcion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inscripcion_Carrera_CarreraId",
                        column: x => x.CarreraId,
                        principalTable: "Carrera",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inscripcion_Corredor_CorredorId",
                        column: x => x.CorredorId,
                        principalTable: "Corredor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inscripcion_Registro_RegistroId",
                        column: x => x.RegistroId,
                        principalTable: "Registro",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inscripcion_CarreraId",
                table: "Inscripcion",
                column: "CarreraId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripcion_CorredorId",
                table: "Inscripcion",
                column: "CorredorId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripcion_RegistroId",
                table: "Inscripcion",
                column: "RegistroId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inscripcion");
        }
    }
}
