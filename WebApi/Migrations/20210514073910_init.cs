using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace WebApi.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DayMenu",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayMenu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recipe",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Weight = table.Column<int>(type: "integer", nullable: false),
                    Colories = table.Column<int>(type: "integer", nullable: false),
                    MainPicture = table.Column<byte[]>(type: "bytea", nullable: true),
                    Proteins = table.Column<int>(type: "integer", nullable: false),
                    Greases = table.Column<int>(type: "integer", nullable: false),
                    Carbohydrates = table.Column<int>(type: "integer", nullable: false),
                    HaveMeat = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipe", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PictureList",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    RecipeId = table.Column<long>(type: "bigint", nullable: true),
                    Picture = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PictureList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PictureList_Recipe_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecipeList",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DayUsage = table.Column<bool[]>(type: "boolean[]", nullable: true),
                    RecipeId = table.Column<long>(type: "bigint", nullable: true),
                    DayMenuId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeList_DayMenu_DayMenuId",
                        column: x => x.DayMenuId,
                        principalTable: "DayMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeList_Recipe_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PictureList_RecipeId",
                table: "PictureList",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeList_DayMenuId",
                table: "RecipeList",
                column: "DayMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeList_RecipeId",
                table: "RecipeList",
                column: "RecipeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PictureList");

            migrationBuilder.DropTable(
                name: "RecipeList");

            migrationBuilder.DropTable(
                name: "DayMenu");

            migrationBuilder.DropTable(
                name: "Recipe");
        }
    }
}
