using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Template.Infrastructure.Migrations
{
	public partial class LinkCategoryContentManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Contents_ContentId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ContentId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ContentId",
                table: "Categories");

            migrationBuilder.CreateTable(
                name: "CategoryContent",
                columns: table => new
                {
                    CategoriesId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryContent", x => new { x.CategoriesId, x.ContentsId });
                    table.ForeignKey(
                        name: "FK_CategoryContent_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryContent_Contents_ContentsId",
                        column: x => x.ContentsId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryContent_ContentsId",
                table: "CategoryContent",
                column: "ContentsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryContent");

            migrationBuilder.AddColumn<Guid>(
                name: "ContentId",
                table: "Categories",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ContentId",
                table: "Categories",
                column: "ContentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Contents_ContentId",
                table: "Categories",
                column: "ContentId",
                principalTable: "Contents",
                principalColumn: "Id");
        }
    }
}
