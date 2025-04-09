using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourWebsite.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastTourSiteID",
                table: "TourSites",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "TourSites",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NextTourSiteID",
                table: "TourSites",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "TourSites",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<int>(
                name: "UniqueClicks",
                table: "TourSites",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UniqueVisitors",
                table: "TourSites",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "VisitorSeconds",
                table: "TourSites",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastTourSiteID",
                table: "TourSites");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "TourSites");

            migrationBuilder.DropColumn(
                name: "NextTourSiteID",
                table: "TourSites");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "TourSites");

            migrationBuilder.DropColumn(
                name: "UniqueClicks",
                table: "TourSites");

            migrationBuilder.DropColumn(
                name: "UniqueVisitors",
                table: "TourSites");

            migrationBuilder.DropColumn(
                name: "VisitorSeconds",
                table: "TourSites");
        }
    }
}
