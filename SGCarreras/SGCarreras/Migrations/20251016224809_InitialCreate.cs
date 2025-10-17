using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGCarreras.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carrera",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", nullable: false),
                    nombre = table.Column<string>(type: "TEXT", nullable: false),
                    ubicacion = table.Column<string>(type: "TEXT", nullable: false),
                    estado = table.Column<int>(type: "INTEGER", nullable: false),
                    kmTotales = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carrera", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Corredor",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", nullable: false),
                    sexo = table.Column<int>(type: "INTEGER", nullable: false),
                    nombreCompleto = table.Column<string>(type: "TEXT", nullable: false),
                    cedula = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Corredor", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PuntoDeControl",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    distancia = table.Column<double>(type: "REAL", nullable: false),
                    Carreraid = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PuntoDeControl", x => x.id);
                    table.ForeignKey(
                        name: "FK_PuntoDeControl_Carrera_Carreraid",
                        column: x => x.Carreraid,
                        principalTable: "Carrera",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Registro",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    numeroEnCarrera = table.Column<int>(type: "INTEGER", nullable: false),
                    posicionEnCarrera = table.Column<int>(type: "INTEGER", nullable: false),
                    horaDeFinalizacion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Corredorid = table.Column<string>(type: "TEXT", nullable: false),
                    Carreraid = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registro", x => x.id);
                    table.ForeignKey(
                        name: "FK_Registro_Carrera_Carreraid",
                        column: x => x.Carreraid,
                        principalTable: "Carrera",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Registro_Corredor_Corredorid",
                        column: x => x.Corredorid,
                        principalTable: "Corredor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TiempoParcial",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    tiempo = table.Column<int>(type: "INTEGER", nullable: false),
                    PuntoControlid = table.Column<int>(type: "INTEGER", nullable: false),
                    Registroid = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiempoParcial", x => x.id);
                    table.ForeignKey(
                        name: "FK_TiempoParcial_PuntoDeControl_PuntoControlid",
                        column: x => x.PuntoControlid,
                        principalTable: "PuntoDeControl",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TiempoParcial_Registro_Registroid",
                        column: x => x.Registroid,
                        principalTable: "Registro",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PuntoDeControl_Carreraid",
                table: "PuntoDeControl",
                column: "Carreraid");

            migrationBuilder.CreateIndex(
                name: "IX_Registro_Carreraid",
                table: "Registro",
                column: "Carreraid");

            migrationBuilder.CreateIndex(
                name: "IX_Registro_Corredorid",
                table: "Registro",
                column: "Corredorid");

            migrationBuilder.CreateIndex(
                name: "IX_TiempoParcial_PuntoControlid",
                table: "TiempoParcial",
                column: "PuntoControlid");

            migrationBuilder.CreateIndex(
                name: "IX_TiempoParcial_Registroid",
                table: "TiempoParcial",
                column: "Registroid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TiempoParcial");

            migrationBuilder.DropTable(
                name: "PuntoDeControl");

            migrationBuilder.DropTable(
                name: "Registro");

            migrationBuilder.DropTable(
                name: "Carrera");

            migrationBuilder.DropTable(
                name: "Corredor");
        }
    }
}
