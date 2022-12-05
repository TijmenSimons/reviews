using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Template.Infrastructure.Migrations
{
    public partial class AddParentCategoryToDefaultCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DefaultCategories_DefaultCategories_ParentCategoryId",
                table: "DefaultCategories");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentCategoryId",
                table: "DefaultCategories",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_DefaultCategories_DefaultCategories_ParentCategoryId",
                table: "DefaultCategories",
                column: "ParentCategoryId",
                principalTable: "DefaultCategories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DefaultCategories_DefaultCategories_ParentCategoryId",
                table: "DefaultCategories");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentCategoryId",
                table: "DefaultCategories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DefaultCategories_DefaultCategories_ParentCategoryId",
                table: "DefaultCategories",
                column: "ParentCategoryId",
                principalTable: "DefaultCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
