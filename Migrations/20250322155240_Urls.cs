using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourWebsite.Migrations
{
    /// <inheritdoc />
    public partial class Urls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmbedUrl",
                table: "UploadedFiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmbedUrl",
                table: "UploadedFiles");
        }
    }
}
