using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourWebsite.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastTourSiteID",
                table: "TourSites");

            migrationBuilder.AddColumn<string>(
                name: "LastTourSiteIDs",
                table: "TourSites",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastTourSiteIDs",
                table: "TourSites");

            migrationBuilder.AddColumn<string>(
                name: "LastTourSiteID",
                table: "TourSites",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
