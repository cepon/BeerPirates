using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeerRecommendations.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BeerRecommendations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeerRecommendations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BeerRecommendationProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<int>(type: "int", nullable: false),
                    BeerRecommendationId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeerRecommendationProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BeerRecommendationProducts_BeerRecommendations_BeerRecommendationId",
                        column: x => x.BeerRecommendationId,
                        principalTable: "BeerRecommendations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BeerRecommendationProducts_BeerRecommendationId",
                table: "BeerRecommendationProducts",
                column: "BeerRecommendationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BeerRecommendationProducts");

            migrationBuilder.DropTable(
                name: "BeerRecommendations");
        }
    }
}
