using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieRecommendation.Migrations
{
    /// <inheritdoc />
    public partial class fixedtypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AvarageRating",
                table: "Movies",
                newName: "AverageRating");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AverageRating",
                table: "Movies",
                newName: "AvarageRating");
        }
    }
}
