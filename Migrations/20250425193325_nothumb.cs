using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourWebsite.Migrations
{
    /// <inheritdoc />
    public partial class nothumb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailID",
                table: "NonTourPage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThumbnailID",
                table: "NonTourPage",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
