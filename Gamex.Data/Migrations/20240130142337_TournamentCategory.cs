using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gamex.Data.Migrations
{
    /// <inheritdoc />
    public partial class TournamentCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "UserTournaments",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateJoined",
                table: "UserTournaments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TournamentCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TournamentTournamentCategory",
                columns: table => new
                {
                    CategoriesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TournamentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentTournamentCategory", x => new { x.CategoriesId, x.TournamentsId });
                    table.ForeignKey(
                        name: "FK_TournamentTournamentCategory_TournamentCategories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "TournamentCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TournamentTournamentCategory_Tournaments_TournamentsId",
                        column: x => x.TournamentsId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TournamentTournamentCategory_TournamentsId",
                table: "TournamentTournamentCategory",
                column: "TournamentsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TournamentTournamentCategory");

            migrationBuilder.DropTable(
                name: "TournamentCategories");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "UserTournaments");

            migrationBuilder.DropColumn(
                name: "DateJoined",
                table: "UserTournaments");
        }
    }
}
