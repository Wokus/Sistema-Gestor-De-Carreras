using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGCarreras.Migrations
{
    /// <inheritdoc />
    public partial class version1_7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "confirmado",
                table: "Registro",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "confirmado",
                table: "Registro");
        }
    }
}
