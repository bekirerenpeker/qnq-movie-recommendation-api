using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieRecommendation.Migrations
{
    /// <inheritdoc />
    public partial class addedwatchlisttodb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MovieDataUserData",
                columns: table => new
                {
                    WatchedMoviesId = table.Column<Guid>(type: "uuid", nullable: false),
                    WatchedUsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieDataUserData", x => new { x.WatchedMoviesId, x.WatchedUsersId });
                    table.ForeignKey(
                        name: "FK_MovieDataUserData_Movies_WatchedMoviesId",
                        column: x => x.WatchedMoviesId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieDataUserData_Users_WatchedUsersId",
                        column: x => x.WatchedUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieDataUserData_WatchedUsersId",
                table: "MovieDataUserData",
                column: "WatchedUsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieDataUserData");
        }
    }
}
