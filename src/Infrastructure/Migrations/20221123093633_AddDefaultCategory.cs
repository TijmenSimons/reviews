using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Template.Infrastructure.Migrations
{
    public partial class AddDefaultCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Categories");

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultCategoryId",
                table: "Categories",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultCategoryId1",
                table: "Categories",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DefaultCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ImageId = table.Column<Guid>(type: "uuid", nullable: true),
                    PartnerId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsRoot = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefaultCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DefaultCategories_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DefaultCategories_Partners_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "Partners",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_DefaultCategoryId",
                table: "Categories",
                column: "DefaultCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_DefaultCategoryId1",
                table: "Categories",
                column: "DefaultCategoryId1");

            migrationBuilder.CreateIndex(
                name: "IX_DefaultCategories_ImageId",
                table: "DefaultCategories",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_DefaultCategories_PartnerId",
                table: "DefaultCategories",
                column: "PartnerId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_DefaultCategories_DefaultCategoryId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_DefaultCategories_DefaultCategoryId1",
                table: "Categories");

            migrationBuilder.DropTable(
                name: "DefaultCategories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_DefaultCategoryId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_DefaultCategoryId1",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DefaultCategoryId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DefaultCategoryId1",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Categories",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
