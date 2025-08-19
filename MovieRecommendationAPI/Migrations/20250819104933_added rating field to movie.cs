using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieRecommendation.Migrations
{
    /// <inheritdoc />
    public partial class addedratingfieldtomovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "AvarageRating",
                table: "Movies",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvarageRating",
                table: "Movies");
        }
    }
}
