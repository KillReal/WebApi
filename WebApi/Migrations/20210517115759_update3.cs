using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class update3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HaveMeat",
                table: "Recipe",
                newName: "IsVegetarian");

            migrationBuilder.AddColumn<bool>(
                name: "IsVegan",
                table: "Recipe",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVegan",
                table: "Recipe");

            migrationBuilder.RenameColumn(
                name: "IsVegetarian",
                table: "Recipe",
                newName: "HaveMeat");
        }
    }
}
