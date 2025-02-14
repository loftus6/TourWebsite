using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourWebsite.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coordinates",
                table: "TourSites");

            migrationBuilder.AddColumn<decimal>(
                name: "Lattitude",
                table: "TourSites",
                type: "decimal(25,8)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "TourSites",
                type: "decimal(25,8)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lattitude",
                table: "TourSites");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "TourSites");

            migrationBuilder.AddColumn<string>(
                name: "Coordinates",
                table: "TourSites",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
