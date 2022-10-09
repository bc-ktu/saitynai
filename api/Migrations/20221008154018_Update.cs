using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderForeignKey",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "OrderForeignKey",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
