using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace WebApi.Migrations
{
    public partial class daymenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DayMenuId",
                table: "Recipe",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DayMenu",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayMenu", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_DayMenuId",
                table: "Recipe",
                column: "DayMenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_DayMenu_DayMenuId",
                table: "Recipe",
                column: "DayMenuId",
                principalTable: "DayMenu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_DayMenu_DayMenuId",
                table: "Recipe");

            migrationBuilder.DropTable(
                name: "DayMenu");

            migrationBuilder.DropIndex(
                name: "IX_Recipe_DayMenuId",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "DayMenuId",
                table: "Recipe");
        }
    }
}
