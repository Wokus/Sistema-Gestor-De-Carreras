using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGCarreras.Migrations
{
    /// <inheritdoc />
    public partial class AddPuntosDeControlTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PuntoDeControl_Carrera_CarreraId",
                table: "PuntoDeControl");

            migrationBuilder.DropForeignKey(
                name: "FK_TiempoParcial_PuntoDeControl_PuntoControlId",
                table: "TiempoParcial");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PuntoDeControl",
                table: "PuntoDeControl");

            migrationBuilder.RenameTable(
                name: "PuntoDeControl",
                newName: "PuntosDeControl");

            migrationBuilder.RenameIndex(
                name: "IX_PuntoDeControl_CarreraId",
                table: "PuntosDeControl",
                newName: "IX_PuntosDeControl_CarreraId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PuntosDeControl",
                table: "PuntosDeControl",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_PuntosDeControl_Carrera_CarreraId",
                table: "PuntosDeControl",
                column: "CarreraId",
                principalTable: "Carrera",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TiempoParcial_PuntosDeControl_PuntoControlId",
                table: "TiempoParcial",
                column: "PuntoControlId",
                principalTable: "PuntosDeControl",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PuntosDeControl_Carrera_CarreraId",
                table: "PuntosDeControl");

            migrationBuilder.DropForeignKey(
                name: "FK_TiempoParcial_PuntosDeControl_PuntoControlId",
                table: "TiempoParcial");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PuntosDeControl",
                table: "PuntosDeControl");

            migrationBuilder.RenameTable(
                name: "PuntosDeControl",
                newName: "PuntoDeControl");

            migrationBuilder.RenameIndex(
                name: "IX_PuntosDeControl_CarreraId",
                table: "PuntoDeControl",
                newName: "IX_PuntoDeControl_CarreraId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PuntoDeControl",
                table: "PuntoDeControl",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_PuntoDeControl_Carrera_CarreraId",
                table: "PuntoDeControl",
                column: "CarreraId",
                principalTable: "Carrera",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TiempoParcial_PuntoDeControl_PuntoControlId",
                table: "TiempoParcial",
                column: "PuntoControlId",
                principalTable: "PuntoDeControl",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
