using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DayMenu_RecipeList_RecipeListId",
                table: "DayMenu");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_RecipeList_RecipeListId",
                table: "Recipe");

            migrationBuilder.DropIndex(
                name: "IX_Recipe_RecipeListId",
                table: "Recipe");

            migrationBuilder.DropIndex(
                name: "IX_DayMenu_RecipeListId",
                table: "DayMenu");

            migrationBuilder.DropColumn(
                name: "RecipeListId",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "RecipeListId",
                table: "DayMenu");

            migrationBuilder.AddColumn<long>(
                name: "DayMenuId",
                table: "RecipeList",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RecipeId",
                table: "RecipeList",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeList_DayMenuId",
                table: "RecipeList",
                column: "DayMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeList_RecipeId",
                table: "RecipeList",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeList_DayMenu_DayMenuId",
                table: "RecipeList",
                column: "DayMenuId",
                principalTable: "DayMenu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeList_Recipe_RecipeId",
                table: "RecipeList",
                column: "RecipeId",
                principalTable: "Recipe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeList_DayMenu_DayMenuId",
                table: "RecipeList");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeList_Recipe_RecipeId",
                table: "RecipeList");

            migrationBuilder.DropIndex(
                name: "IX_RecipeList_DayMenuId",
                table: "RecipeList");

            migrationBuilder.DropIndex(
                name: "IX_RecipeList_RecipeId",
                table: "RecipeList");

            migrationBuilder.DropColumn(
                name: "DayMenuId",
                table: "RecipeList");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "RecipeList");

            migrationBuilder.AddColumn<long>(
                name: "RecipeListId",
                table: "Recipe",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RecipeListId",
                table: "DayMenu",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_RecipeListId",
                table: "Recipe",
                column: "RecipeListId");

            migrationBuilder.CreateIndex(
                name: "IX_DayMenu_RecipeListId",
                table: "DayMenu",
                column: "RecipeListId");

            migrationBuilder.AddForeignKey(
                name: "FK_DayMenu_RecipeList_RecipeListId",
                table: "DayMenu",
                column: "RecipeListId",
                principalTable: "RecipeList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_RecipeList_RecipeListId",
                table: "Recipe",
                column: "RecipeListId",
                principalTable: "RecipeList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
