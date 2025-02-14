using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourWebsite.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TourSites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(25,8)", nullable: false),
                    Lattitude = table.Column<decimal>(type: "decimal(25,8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourSites", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TourSites");
        }
    }
}
