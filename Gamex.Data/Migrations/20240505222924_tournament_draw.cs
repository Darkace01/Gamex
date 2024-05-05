using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gamex.Data.Migrations
{
    /// <inheritdoc />
    public partial class tournament_draw : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Draw",
                table: "UserTournaments",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Draw",
                table: "UserTournaments");
        }
    }
}
