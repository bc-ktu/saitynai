using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class FixLowerCases : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isDisplayed",
                table: "Products",
                newName: "IsDisplayed");

            migrationBuilder.RenameColumn(
                name: "canBeBought",
                table: "Products",
                newName: "CanBeBought");

            migrationBuilder.RenameColumn(
                name: "isDeleted",
                table: "Comments",
                newName: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDisplayed",
                table: "Products",
                newName: "isDisplayed");

            migrationBuilder.RenameColumn(
                name: "CanBeBought",
                table: "Products",
                newName: "canBeBought");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Comments",
                newName: "isDeleted");
        }
    }
}
