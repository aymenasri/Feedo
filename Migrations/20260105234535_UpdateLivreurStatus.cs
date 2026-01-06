using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Feedo.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLivreurStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Livreur");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Livreur",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Livreur");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Livreur",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
