using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gamex.Data.Migrations
{
    /// <inheritdoc />
    public partial class Tournament_Rounds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TournamentRounds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TournamentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentRounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentRounds_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoundMatches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TournamentRoundId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoundMatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoundMatches_TournamentRounds_TournamentRoundId",
                        column: x => x.TournamentRoundId,
                        principalTable: "TournamentRounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Point = table.Column<int>(type: "int", nullable: true),
                    Win = table.Column<bool>(type: "bit", nullable: true),
                    Loss = table.Column<bool>(type: "bit", nullable: true),
                    Draw = table.Column<bool>(type: "bit", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatchUsers_RoundMatches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "RoundMatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatchUsers_MatchId",
                table: "MatchUsers",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchUsers_UserId",
                table: "MatchUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoundMatches_TournamentRoundId",
                table: "RoundMatches",
                column: "TournamentRoundId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentRounds_TournamentId",
                table: "TournamentRounds",
                column: "TournamentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchUsers");

            migrationBuilder.DropTable(
                name: "RoundMatches");

            migrationBuilder.DropTable(
                name: "TournamentRounds");
        }
    }
}
