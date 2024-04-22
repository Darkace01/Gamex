using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gamex.Data.Migrations
{
    /// <inheritdoc />
    public partial class Leaderboard_Win_Loss_WaitList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Loss",
                table: "UserTournaments",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "WaitList",
                table: "UserTournaments",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Win",
                table: "UserTournaments",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Loss",
                table: "UserTournaments");

            migrationBuilder.DropColumn(
                name: "WaitList",
                table: "UserTournaments");

            migrationBuilder.DropColumn(
                name: "Win",
                table: "UserTournaments");
        }
    }
}
