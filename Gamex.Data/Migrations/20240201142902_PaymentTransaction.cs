using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gamex.Data.Migrations
{
    /// <inheritdoc />
    public partial class PaymentTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PaymentTransactionId",
                table: "UserTournaments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TournamentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TransactionReference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentTransactions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentTransactions_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTournaments_PaymentTransactionId",
                table: "UserTournaments",
                column: "PaymentTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_TournamentId",
                table: "PaymentTransactions",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_UserId",
                table: "PaymentTransactions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTournaments_PaymentTransactions_PaymentTransactionId",
                table: "UserTournaments",
                column: "PaymentTransactionId",
                principalTable: "PaymentTransactions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTournaments_PaymentTransactions_PaymentTransactionId",
                table: "UserTournaments");

            migrationBuilder.DropTable(
                name: "PaymentTransactions");

            migrationBuilder.DropIndex(
                name: "IX_UserTournaments_PaymentTransactionId",
                table: "UserTournaments");

            migrationBuilder.DropColumn(
                name: "PaymentTransactionId",
                table: "UserTournaments");
        }
    }
}
