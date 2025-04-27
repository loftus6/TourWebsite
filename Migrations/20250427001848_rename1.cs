using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourWebsite.Migrations
{
    /// <inheritdoc />
    public partial class rename1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ColorName1",
                table: "NonTourPage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ColorName2",
                table: "NonTourPage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorName1",
                table: "NonTourPage");

            migrationBuilder.DropColumn(
                name: "ColorName2",
                table: "NonTourPage");
        }
    }
}
