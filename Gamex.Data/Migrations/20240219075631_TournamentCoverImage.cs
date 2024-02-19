using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gamex.Data.Migrations
{
    /// <inheritdoc />
    public partial class TournamentCoverImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "UserTournaments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "CoverPictureId",
                table: "Tournaments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_CoverPictureId",
                table: "Tournaments",
                column: "CoverPictureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Pictures_CoverPictureId",
                table: "Tournaments",
                column: "CoverPictureId",
                principalTable: "Pictures",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Pictures_CoverPictureId",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_CoverPictureId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "UserTournaments");

            migrationBuilder.DropColumn(
                name: "CoverPictureId",
                table: "Tournaments");
        }
    }
}
