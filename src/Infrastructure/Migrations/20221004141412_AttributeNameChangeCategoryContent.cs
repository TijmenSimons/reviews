using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Template.Infrastructure.Migrations
{
    public partial class AttributeNameChangeCategoryContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryContent_Categories_CategoriesId",
                table: "CategoryContent");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryContent_Contents_ContentsId",
                table: "CategoryContent");

            migrationBuilder.RenameColumn(
                name: "ContentsId",
                table: "CategoryContent",
                newName: "ContentId");

            migrationBuilder.RenameColumn(
                name: "CategoriesId",
                table: "CategoryContent",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryContent_ContentsId",
                table: "CategoryContent",
                newName: "IX_CategoryContent_ContentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryContent_Categories_CategoryId",
                table: "CategoryContent",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryContent_Contents_ContentId",
                table: "CategoryContent",
                column: "ContentId",
                principalTable: "Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryContent_Categories_CategoryId",
                table: "CategoryContent");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryContent_Contents_ContentId",
                table: "CategoryContent");

            migrationBuilder.RenameColumn(
                name: "ContentId",
                table: "CategoryContent",
                newName: "ContentsId");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "CategoryContent",
                newName: "CategoriesId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryContent_ContentId",
                table: "CategoryContent",
                newName: "IX_CategoryContent_ContentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryContent_Categories_CategoriesId",
                table: "CategoryContent",
                column: "CategoriesId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryContent_Contents_ContentsId",
                table: "CategoryContent",
                column: "ContentsId",
                principalTable: "Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
