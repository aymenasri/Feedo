using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Feedo.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailToPersonne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Utilisateur",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Livreur",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Client",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Utilisateur");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Livreur");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Client");
        }
    }
}
