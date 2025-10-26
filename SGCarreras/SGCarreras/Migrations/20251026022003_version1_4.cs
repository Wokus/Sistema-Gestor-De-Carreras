using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGCarreras.Migrations
{
    /// <inheritdoc />
    public partial class version1_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "contra",
                table: "Corredor",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "mail",
                table: "Corredor",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Corredor_mail",
                table: "Corredor",
                column: "mail",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Corredor_mail",
                table: "Corredor");

            migrationBuilder.DropColumn(
                name: "contra",
                table: "Corredor");

            migrationBuilder.DropColumn(
                name: "mail",
                table: "Corredor");
        }
    }
}
