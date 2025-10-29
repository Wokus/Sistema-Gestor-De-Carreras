using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGCarreras.Migrations
{
    /// <inheritdoc />
    public partial class MIGRACION : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "tiempo",
                table: "TiempoParcial",
                newName: "Tiempo");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "TiempoParcial",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "posicionEnCarrera",
                table: "Registro",
                newName: "PosicionEnCarrera");

            migrationBuilder.RenameColumn(
                name: "numeroEnCarrera",
                table: "Registro",
                newName: "NumeroEnCarrera");

            migrationBuilder.RenameColumn(
                name: "horaDeFinalizacion",
                table: "Registro",
                newName: "HoraDeFinalizacion");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Registro",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "distancia",
                table: "PuntoDeControl",
                newName: "Distancia");

            migrationBuilder.RenameColumn(
                name: "sexo",
                table: "Corredor",
                newName: "Sexo");

            migrationBuilder.RenameColumn(
                name: "nombreCompleto",
                table: "Corredor",
                newName: "NombreCompleto");

            migrationBuilder.RenameColumn(
                name: "mail",
                table: "Corredor",
                newName: "Mail");

            migrationBuilder.RenameColumn(
                name: "contra",
                table: "Corredor",
                newName: "Contra");

            migrationBuilder.RenameColumn(
                name: "cedula",
                table: "Corredor",
                newName: "Cedula");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Corredor",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Corredor_mail",
                table: "Corredor",
                newName: "IX_Corredor_Mail");

            migrationBuilder.RenameColumn(
                name: "ubicacion",
                table: "Carrera",
                newName: "Ubicacion");

            migrationBuilder.RenameColumn(
                name: "nombre",
                table: "Carrera",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "kmTotales",
                table: "Carrera",
                newName: "KmTotales");

            migrationBuilder.RenameColumn(
                name: "estado",
                table: "Carrera",
                newName: "Estado");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Carrera",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Nacionalidad",
                table: "Corredor",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nacionalidad",
                table: "Corredor");

            migrationBuilder.RenameColumn(
                name: "Tiempo",
                table: "TiempoParcial",
                newName: "tiempo");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "TiempoParcial",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "PosicionEnCarrera",
                table: "Registro",
                newName: "posicionEnCarrera");

            migrationBuilder.RenameColumn(
                name: "NumeroEnCarrera",
                table: "Registro",
                newName: "numeroEnCarrera");

            migrationBuilder.RenameColumn(
                name: "HoraDeFinalizacion",
                table: "Registro",
                newName: "horaDeFinalizacion");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Registro",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Distancia",
                table: "PuntoDeControl",
                newName: "distancia");

            migrationBuilder.RenameColumn(
                name: "Sexo",
                table: "Corredor",
                newName: "sexo");

            migrationBuilder.RenameColumn(
                name: "NombreCompleto",
                table: "Corredor",
                newName: "nombreCompleto");

            migrationBuilder.RenameColumn(
                name: "Mail",
                table: "Corredor",
                newName: "mail");

            migrationBuilder.RenameColumn(
                name: "Contra",
                table: "Corredor",
                newName: "contra");

            migrationBuilder.RenameColumn(
                name: "Cedula",
                table: "Corredor",
                newName: "cedula");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Corredor",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Corredor_Mail",
                table: "Corredor",
                newName: "IX_Corredor_mail");

            migrationBuilder.RenameColumn(
                name: "Ubicacion",
                table: "Carrera",
                newName: "ubicacion");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Carrera",
                newName: "nombre");

            migrationBuilder.RenameColumn(
                name: "KmTotales",
                table: "Carrera",
                newName: "kmTotales");

            migrationBuilder.RenameColumn(
                name: "Estado",
                table: "Carrera",
                newName: "estado");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Carrera",
                newName: "id");
        }
    }
}
