using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Feedo.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCommandeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Couriers_CourierId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Commande");

            migrationBuilder.DropTable(
                name: "Couriers");

            migrationBuilder.RenameColumn(
                name: "CourierId",
                table: "Orders",
                newName: "LivreurId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CourierId",
                table: "Orders",
                newName: "IX_Orders_LivreurId");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Livreur",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastDeliveryAt",
                table: "Livreur",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LicensePlate",
                table: "Livreur",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Rating",
                table: "Livreur",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TotalDeliveries",
                table: "Livreur",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "VehicleType",
                table: "Livreur",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Livreur_LivreurId",
                table: "Orders",
                column: "LivreurId",
                principalTable: "Livreur",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Livreur_LivreurId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Livreur");

            migrationBuilder.DropColumn(
                name: "LastDeliveryAt",
                table: "Livreur");

            migrationBuilder.DropColumn(
                name: "LicensePlate",
                table: "Livreur");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Livreur");

            migrationBuilder.DropColumn(
                name: "TotalDeliveries",
                table: "Livreur");

            migrationBuilder.DropColumn(
                name: "VehicleType",
                table: "Livreur");

            migrationBuilder.RenameColumn(
                name: "LivreurId",
                table: "Orders",
                newName: "CourierId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_LivreurId",
                table: "Orders",
                newName: "IX_Orders_CourierId");

            migrationBuilder.CreateTable(
                name: "Commande",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    CreationA = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstSupprimer = table.Column<bool>(type: "bit", nullable: false),
                    LivreurId = table.Column<int>(type: "int", nullable: false),
                    NCommande = table.Column<int>(type: "int", nullable: false),
                    Statut = table.Column<int>(type: "int", nullable: false),
                    SupperimeA = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupperimerPar = table.Column<int>(type: "int", nullable: false),
                    TotalHT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTTC = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commande", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Couriers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Compte = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationA = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    EstSupprimer = table.Column<bool>(type: "bit", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    LastDeliveryAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LicensePlate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotPasse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NumeroTelephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SupperimeA = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupperimerPar = table.Column<int>(type: "int", nullable: false),
                    TotalDeliveries = table.Column<int>(type: "int", nullable: false),
                    VehicleType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Couriers", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Couriers_CourierId",
                table: "Orders",
                column: "CourierId",
                principalTable: "Couriers",
                principalColumn: "Id");
        }
    }
}
