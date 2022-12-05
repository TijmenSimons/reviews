using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Template.Infrastructure.Migrations
{
    public partial class OneToManyCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryCategory");

            migrationBuilder.DropTable(
                name: "DefaultCategoryDefaultCategory");

            migrationBuilder.AddColumn<Guid>(
                name: "ParentCategoryId",
                table: "DefaultCategories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ParentCategoryId",
                table: "Categories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DefaultCategories_ParentCategoryId",
                table: "DefaultCategories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentCategoryId",
                table: "Categories",
                column: "ParentCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Categories_ParentCategoryId",
                table: "Categories",
                column: "ParentCategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DefaultCategories_DefaultCategories_ParentCategoryId",
                table: "DefaultCategories",
                column: "ParentCategoryId",
                principalTable: "DefaultCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Categories_ParentCategoryId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_DefaultCategories_DefaultCategories_ParentCategoryId",
                table: "DefaultCategories");

            migrationBuilder.DropIndex(
                name: "IX_DefaultCategories_ParentCategoryId",
                table: "DefaultCategories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ParentCategoryId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ParentCategoryId",
                table: "DefaultCategories");

            migrationBuilder.DropColumn(
                name: "ParentCategoryId",
                table: "Categories");

            migrationBuilder.CreateTable(
                name: "CategoryCategory",
                columns: table => new
                {
                    ChildCategoriesId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentCategoriesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryCategory", x => new { x.ChildCategoriesId, x.ParentCategoriesId });
                    table.ForeignKey(
                        name: "FK_CategoryCategory_Categories_ChildCategoriesId",
                        column: x => x.ChildCategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryCategory_Categories_ParentCategoriesId",
                        column: x => x.ParentCategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_CategoryCategory_ParentCategoriesId",
                table: "CategoryCategory",
                column: "ParentCategoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_DefaultCategoryDefaultCategory_ParentCategoriesId",
                table: "DefaultCategoryDefaultCategory",
                column: "ParentCategoriesId");
        }
    }
}
