using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Feedo.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationA = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupperimeA = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstSupprimer = table.Column<bool>(type: "bit", nullable: false),
                    SupperimerPar = table.Column<int>(type: "int", nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NumeroTelephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Compte = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MotPasse = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Commande",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalHT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTTC = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NCommande = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    LivreurId = table.Column<int>(type: "int", nullable: false),
                    Statut = table.Column<int>(type: "int", nullable: false),
                    CreationA = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupperimeA = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstSupprimer = table.Column<bool>(type: "bit", nullable: false),
                    SupperimerPar = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commande", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Livreur",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationA = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupperimeA = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstSupprimer = table.Column<bool>(type: "bit", nullable: false),
                    SupperimerPar = table.Column<int>(type: "int", nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NumeroTelephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Compte = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MotPasse = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livreur", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Utilisateur",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationA = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupperimeA = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstSupprimer = table.Column<bool>(type: "bit", nullable: false),
                    SupperimerPar = table.Column<int>(type: "int", nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NumeroTelephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Compte = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MotPasse = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilisateur", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Commande");

            migrationBuilder.DropTable(
                name: "Livreur");

            migrationBuilder.DropTable(
                name: "Utilisateur");
        }
    }
}
