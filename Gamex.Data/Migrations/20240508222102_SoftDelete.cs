using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gamex.Data.Migrations
{
    /// <inheritdoc />
    public partial class SoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                table: "UserTournaments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserTournaments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                table: "UserConfirmationCodes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserConfirmationCodes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                table: "Tournaments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Tournaments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                table: "TournamentRounds",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TournamentRounds",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                table: "TournamentCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TournamentCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                table: "Tags",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Tags",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                table: "RoundMatches",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RoundMatches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                table: "PostTags",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PostTags",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                table: "Posts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Posts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                table: "Pictures",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Pictures",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                table: "PaymentTransactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PaymentTransactions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                table: "MatchUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "MatchUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                table: "Comments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_UserTournaments_IsDeleted",
                table: "UserTournaments",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_UserConfirmationCodes_IsDeleted",
                table: "UserConfirmationCodes",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_IsDeleted",
                table: "Tournaments",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentRounds_IsDeleted",
                table: "TournamentRounds",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentCategories_IsDeleted",
                table: "TournamentCategories",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_IsDeleted",
                table: "Tags",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_RoundMatches_IsDeleted",
                table: "RoundMatches",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_PostTags_IsDeleted",
                table: "PostTags",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_IsDeleted",
                table: "Posts",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_IsDeleted",
                table: "Pictures",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_IsDeleted",
                table: "PaymentTransactions",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_MatchUsers_IsDeleted",
                table: "MatchUsers",
                column: "IsDeleted",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_IsDeleted",
                table: "Comments",
                column: "IsDeleted",
                filter: "IsDeleted = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserTournaments_IsDeleted",
                table: "UserTournaments");

            migrationBuilder.DropIndex(
                name: "IX_UserConfirmationCodes_IsDeleted",
                table: "UserConfirmationCodes");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_IsDeleted",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_TournamentRounds_IsDeleted",
                table: "TournamentRounds");

            migrationBuilder.DropIndex(
                name: "IX_TournamentCategories_IsDeleted",
                table: "TournamentCategories");

            migrationBuilder.DropIndex(
                name: "IX_Tags_IsDeleted",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_RoundMatches_IsDeleted",
                table: "RoundMatches");

            migrationBuilder.DropIndex(
                name: "IX_PostTags_IsDeleted",
                table: "PostTags");

            migrationBuilder.DropIndex(
                name: "IX_Posts_IsDeleted",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Pictures_IsDeleted",
                table: "Pictures");

            migrationBuilder.DropIndex(
                name: "IX_PaymentTransactions_IsDeleted",
                table: "PaymentTransactions");

            migrationBuilder.DropIndex(
                name: "IX_MatchUsers_IsDeleted",
                table: "MatchUsers");

            migrationBuilder.DropIndex(
                name: "IX_Comments_IsDeleted",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                table: "UserTournaments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserTournaments");

            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                table: "UserConfirmationCodes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserConfirmationCodes");

            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                table: "TournamentRounds");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TournamentRounds");

            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                table: "TournamentCategories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TournamentCategories");

            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                table: "RoundMatches");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RoundMatches");

            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                table: "PostTags");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PostTags");

            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                table: "PaymentTransactions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PaymentTransactions");

            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                table: "MatchUsers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "MatchUsers");

            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Comments");
        }
    }
}
