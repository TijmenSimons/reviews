using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Template.Infrastructure.Migrations
{
    public partial class AddDefaultCategoryToCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_DefaultCategories_DefaultCategoryId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_DefaultCategories_DefaultCategoryId1",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_DefaultCategoryId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DefaultCategoryId",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "DefaultCategoryId1",
                table: "Categories",
                newName: "DefaultValueId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_DefaultCategoryId1",
                table: "Categories",
                newName: "IX_Categories_DefaultValueId");

            migrationBuilder.CreateTable(
                name: "DefaultCategoryDefaultCategory",
                columns: table => new
                {
                    ChildCategoriesId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentCategoriesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefaultCategoryDefaultCategory", x => new { x.ChildCategoriesId, x.ParentCategoriesId });
                    table.ForeignKey(
                        name: "FK_DefaultCategoryDefaultCategory_DefaultCategories_ChildCateg~",
                        column: x => x.ChildCategoriesId,
                        principalTable: "DefaultCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DefaultCategoryDefaultCategory_DefaultCategories_ParentCate~",
                        column: x => x.ParentCategoriesId,
                        principalTable: "DefaultCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DefaultCategoryDefaultCategory_ParentCategoriesId",
                table: "DefaultCategoryDefaultCategory",
                column: "ParentCategoriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_DefaultCategories_DefaultValueId",
                table: "Categories",
                column: "DefaultValueId",
                principalTable: "DefaultCategories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_DefaultCategories_DefaultValueId",
                table: "Categories");

            migrationBuilder.DropTable(
                name: "DefaultCategoryDefaultCategory");

            migrationBuilder.RenameColumn(
                name: "DefaultValueId",
                table: "Categories",
                newName: "DefaultCategoryId1");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_DefaultValueId",
                table: "Categories",
                newName: "IX_Categories_DefaultCategoryId1");

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultCategoryId",
                table: "Categories",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_DefaultCategoryId",
                table: "Categories",
                column: "DefaultCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_DefaultCategories_DefaultCategoryId",
                table: "Categories",
                column: "DefaultCategoryId",
                principalTable: "DefaultCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_DefaultCategories_DefaultCategoryId1",
                table: "Categories",
                column: "DefaultCategoryId1",
                principalTable: "DefaultCategories",
                principalColumn: "Id");
        }
    }
}
