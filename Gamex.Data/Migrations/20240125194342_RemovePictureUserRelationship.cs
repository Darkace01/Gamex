using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gamex.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovePictureUserRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_AspNetUsers_Pictures_PictureId",
            //    table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_AspNetUsers_UserId",
                table: "Pictures");

            migrationBuilder.DropIndex(
                name: "IX_Pictures_UserId",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Pictures");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PictureId",
                table: "AspNetUsers",
                column: "PictureId");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_AspNetUsers_Pictures_PictureId",
            //    table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PictureId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Pictures",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_UserId",
                table: "Pictures",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");


        }
    }
}
