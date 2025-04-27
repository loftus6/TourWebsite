using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourWebsite.Migrations
{
    /// <inheritdoc />
    public partial class rename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BackgroundColor",
                table: "NonTourPage",
                newName: "MainColor");

            migrationBuilder.RenameColumn(
                name: "AccentColor1",
                table: "NonTourPage",
                newName: "AccentColor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MainColor",
                table: "NonTourPage",
                newName: "BackgroundColor");

            migrationBuilder.RenameColumn(
                name: "AccentColor",
                table: "NonTourPage",
                newName: "AccentColor1");
        }
    }
}
