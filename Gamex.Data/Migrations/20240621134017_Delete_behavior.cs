using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gamex.Data.Migrations
{
    /// <inheritdoc />
    public partial class Delete_behavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTransactions_AspNetUsers_UserId",
                table: "PaymentTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_PostTags_Posts_PostId",
                table: "PostTags");

            migrationBuilder.DropForeignKey(
                name: "FK_PostTags_Tags_TagId",
                table: "PostTags");

            migrationBuilder.DropForeignKey(
                name: "FK_RoundMatches_TournamentRounds_TournamentRoundId",
                table: "RoundMatches");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentRounds_Tournaments_TournamentId",
                table: "TournamentRounds");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTournaments_AspNetUsers_UserId",
                table: "UserTournaments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTournaments_Tournaments_TournamentId",
                table: "UserTournaments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTransactions_AspNetUsers_UserId",
                table: "PaymentTransactions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostTags_Posts_PostId",
                table: "PostTags",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostTags_Tags_TagId",
                table: "PostTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoundMatches_TournamentRounds_TournamentRoundId",
                table: "RoundMatches",
                column: "TournamentRoundId",
                principalTable: "TournamentRounds",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentRounds_Tournaments_TournamentId",
                table: "TournamentRounds",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTournaments_AspNetUsers_UserId",
                table: "UserTournaments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTournaments_Tournaments_TournamentId",
                table: "UserTournaments",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTransactions_AspNetUsers_UserId",
                table: "PaymentTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_PostTags_Posts_PostId",
                table: "PostTags");

            migrationBuilder.DropForeignKey(
                name: "FK_PostTags_Tags_TagId",
                table: "PostTags");

            migrationBuilder.DropForeignKey(
                name: "FK_RoundMatches_TournamentRounds_TournamentRoundId",
                table: "RoundMatches");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentRounds_Tournaments_TournamentId",
                table: "TournamentRounds");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTournaments_AspNetUsers_UserId",
                table: "UserTournaments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTournaments_Tournaments_TournamentId",
                table: "UserTournaments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTransactions_AspNetUsers_UserId",
                table: "PaymentTransactions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostTags_Posts_PostId",
                table: "PostTags",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostTags_Tags_TagId",
                table: "PostTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoundMatches_TournamentRounds_TournamentRoundId",
                table: "RoundMatches",
                column: "TournamentRoundId",
                principalTable: "TournamentRounds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentRounds_Tournaments_TournamentId",
                table: "TournamentRounds",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTournaments_AspNetUsers_UserId",
                table: "UserTournaments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTournaments_Tournaments_TournamentId",
                table: "UserTournaments",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
