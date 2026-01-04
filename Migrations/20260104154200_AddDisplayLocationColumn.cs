using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Feedo.Migrations
{
    /// <inheritdoc />
    public partial class AddDisplayLocationColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisplayLocation",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayLocation",
                table: "Products");
        }
    }
}
