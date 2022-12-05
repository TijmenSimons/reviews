using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Template.Infrastructure.Migrations
{
    public partial class UpdateContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryContent_Categories_CategoryId",
                table: "CategoryContent");

            migrationBuilder.DropForeignKey(
                name: "FK_Contents_Videos_VideoId",
                table: "Contents");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "CategoryContent",
                newName: "CategoriesId");

            migrationBuilder.AlterColumn<Guid>(
                name: "VideoId",
                table: "Contents",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Contents",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryContent_Categories_CategoriesId",
                table: "CategoryContent",
                column: "CategoriesId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_Videos_VideoId",
                table: "Contents",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryContent_Categories_CategoriesId",
                table: "CategoryContent");

            migrationBuilder.DropForeignKey(
                name: "FK_Contents_Videos_VideoId",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Contents");

            migrationBuilder.RenameColumn(
                name: "CategoriesId",
                table: "CategoryContent",
                newName: "CategoryId");

            migrationBuilder.AlterColumn<Guid>(
                name: "VideoId",
                table: "Contents",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryContent_Categories_CategoryId",
                table: "CategoryContent",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_Videos_VideoId",
                table: "Contents",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
