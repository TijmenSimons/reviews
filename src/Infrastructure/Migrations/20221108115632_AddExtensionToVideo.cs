using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Template.Infrastructure.Migrations
{
    public partial class AddExtensionToVideo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VideoPath",
                table: "Videos",
                newName: "Extension");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Extension",
                table: "Videos",
                newName: "VideoPath");
        }
    }
}
