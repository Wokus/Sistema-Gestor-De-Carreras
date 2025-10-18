using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGCarreras.Migrations
{
    /// <inheritdoc />
    public partial class version1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PuntoDeControl_Carrera_Carreraid",
                table: "PuntoDeControl");

            migrationBuilder.DropForeignKey(
                name: "FK_Registro_Carrera_Carreraid",
                table: "Registro");

            migrationBuilder.DropForeignKey(
                name: "FK_Registro_Corredor_Corredorid",
                table: "Registro");

            migrationBuilder.DropForeignKey(
                name: "FK_TiempoParcial_PuntoDeControl_PuntoControlid",
                table: "TiempoParcial");

            migrationBuilder.DropForeignKey(
                name: "FK_TiempoParcial_Registro_Registroid",
                table: "TiempoParcial");

            migrationBuilder.RenameColumn(
                name: "Registroid",
                table: "TiempoParcial",
                newName: "RegistroId");

            migrationBuilder.RenameColumn(
                name: "PuntoControlid",
                table: "TiempoParcial",
                newName: "PuntoControlId");

            migrationBuilder.RenameIndex(
                name: "IX_TiempoParcial_Registroid",
                table: "TiempoParcial",
                newName: "IX_TiempoParcial_RegistroId");

            migrationBuilder.RenameIndex(
                name: "IX_TiempoParcial_PuntoControlid",
                table: "TiempoParcial",
                newName: "IX_TiempoParcial_PuntoControlId");

            migrationBuilder.RenameColumn(
                name: "Corredorid",
                table: "Registro",
                newName: "CorredorId");

            migrationBuilder.RenameColumn(
                name: "Carreraid",
                table: "Registro",
                newName: "CarreraId");

            migrationBuilder.RenameIndex(
                name: "IX_Registro_Corredorid",
                table: "Registro",
                newName: "IX_Registro_CorredorId");

            migrationBuilder.RenameIndex(
                name: "IX_Registro_Carreraid",
                table: "Registro",
                newName: "IX_Registro_CarreraId");

            migrationBuilder.RenameColumn(
                name: "Carreraid",
                table: "PuntoDeControl",
                newName: "CarreraId");

            migrationBuilder.RenameIndex(
                name: "IX_PuntoDeControl_Carreraid",
                table: "PuntoDeControl",
                newName: "IX_PuntoDeControl_CarreraId");

            migrationBuilder.AlterColumn<int>(
                name: "CarreraId",
                table: "Registro",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "CarreraId",
                table: "PuntoDeControl",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "Carrera",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddForeignKey(
                name: "FK_PuntoDeControl_Carrera_CarreraId",
                table: "PuntoDeControl",
                column: "CarreraId",
                principalTable: "Carrera",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Registro_Carrera_CarreraId",
                table: "Registro",
                column: "CarreraId",
                principalTable: "Carrera",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Registro_Corredor_CorredorId",
                table: "Registro",
                column: "CorredorId",
                principalTable: "Corredor",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TiempoParcial_PuntoDeControl_PuntoControlId",
                table: "TiempoParcial",
                column: "PuntoControlId",
                principalTable: "PuntoDeControl",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TiempoParcial_Registro_RegistroId",
                table: "TiempoParcial",
                column: "RegistroId",
                principalTable: "Registro",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PuntoDeControl_Carrera_CarreraId",
                table: "PuntoDeControl");

            migrationBuilder.DropForeignKey(
                name: "FK_Registro_Carrera_CarreraId",
                table: "Registro");

            migrationBuilder.DropForeignKey(
                name: "FK_Registro_Corredor_CorredorId",
                table: "Registro");

            migrationBuilder.DropForeignKey(
                name: "FK_TiempoParcial_PuntoDeControl_PuntoControlId",
                table: "TiempoParcial");

            migrationBuilder.DropForeignKey(
                name: "FK_TiempoParcial_Registro_RegistroId",
                table: "TiempoParcial");

            migrationBuilder.RenameColumn(
                name: "RegistroId",
                table: "TiempoParcial",
                newName: "Registroid");

            migrationBuilder.RenameColumn(
                name: "PuntoControlId",
                table: "TiempoParcial",
                newName: "PuntoControlid");

            migrationBuilder.RenameIndex(
                name: "IX_TiempoParcial_RegistroId",
                table: "TiempoParcial",
                newName: "IX_TiempoParcial_Registroid");

            migrationBuilder.RenameIndex(
                name: "IX_TiempoParcial_PuntoControlId",
                table: "TiempoParcial",
                newName: "IX_TiempoParcial_PuntoControlid");

            migrationBuilder.RenameColumn(
                name: "CorredorId",
                table: "Registro",
                newName: "Corredorid");

            migrationBuilder.RenameColumn(
                name: "CarreraId",
                table: "Registro",
                newName: "Carreraid");

            migrationBuilder.RenameIndex(
                name: "IX_Registro_CorredorId",
                table: "Registro",
                newName: "IX_Registro_Corredorid");

            migrationBuilder.RenameIndex(
                name: "IX_Registro_CarreraId",
                table: "Registro",
                newName: "IX_Registro_Carreraid");

            migrationBuilder.RenameColumn(
                name: "CarreraId",
                table: "PuntoDeControl",
                newName: "Carreraid");

            migrationBuilder.RenameIndex(
                name: "IX_PuntoDeControl_CarreraId",
                table: "PuntoDeControl",
                newName: "IX_PuntoDeControl_Carreraid");

            migrationBuilder.AlterColumn<string>(
                name: "Carreraid",
                table: "Registro",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Carreraid",
                table: "PuntoDeControl",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                table: "Carrera",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddForeignKey(
                name: "FK_PuntoDeControl_Carrera_Carreraid",
                table: "PuntoDeControl",
                column: "Carreraid",
                principalTable: "Carrera",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Registro_Carrera_Carreraid",
                table: "Registro",
                column: "Carreraid",
                principalTable: "Carrera",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Registro_Corredor_Corredorid",
                table: "Registro",
                column: "Corredorid",
                principalTable: "Corredor",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TiempoParcial_PuntoDeControl_PuntoControlid",
                table: "TiempoParcial",
                column: "PuntoControlid",
                principalTable: "PuntoDeControl",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TiempoParcial_Registro_Registroid",
                table: "TiempoParcial",
                column: "Registroid",
                principalTable: "Registro",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
