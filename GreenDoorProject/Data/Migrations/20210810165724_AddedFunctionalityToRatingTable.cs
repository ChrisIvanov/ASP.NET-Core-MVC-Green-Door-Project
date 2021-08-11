using Microsoft.EntityFrameworkCore.Migrations;

namespace GreenDoorProject.Data.Migrations
{
    public partial class AddedFunctionalityToRatingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserHasVoted",
                table: "Ratings");

            migrationBuilder.AddColumn<string>(
                name: "RatingId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RatingId",
                table: "AspNetUsers",
                column: "RatingId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Ratings_RatingId",
                table: "AspNetUsers",
                column: "RatingId",
                principalTable: "Ratings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Ratings_RatingId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RatingId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RatingId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "UserHasVoted",
                table: "Ratings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
