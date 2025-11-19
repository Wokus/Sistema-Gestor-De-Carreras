using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGCarreras.Migrations
{
    /// <inheritdoc />
    public partial class migracionInscripcion2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TiempoParcial");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TiempoParcial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PuntoControlId = table.Column<int>(type: "INTEGER", nullable: false),
                    RegistroId = table.Column<int>(type: "INTEGER", nullable: false),
                    Tiempo = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiempoParcial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TiempoParcial_PuntosDeControl_PuntoControlId",
                        column: x => x.PuntoControlId,
                        principalTable: "PuntosDeControl",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TiempoParcial_Registro_RegistroId",
                        column: x => x.RegistroId,
                        principalTable: "Registro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TiempoParcial_PuntoControlId",
                table: "TiempoParcial",
                column: "PuntoControlId");

            migrationBuilder.CreateIndex(
                name: "IX_TiempoParcial_RegistroId",
                table: "TiempoParcial",
                column: "RegistroId");
        }
    }
}
