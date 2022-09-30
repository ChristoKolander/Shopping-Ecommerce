using Microsoft.EntityFrameworkCore.Migrations;

namespace Shopping.Api.Migrations
{
    public partial class AddedPropertiesUsernCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserStringId",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CartStringId",
                table: "Carts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserStringId",
                table: "Carts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CartStringId",
                table: "CartItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserStringId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CartStringId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "UserStringId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "CartStringId",
                table: "CartItems");
        }
    }
}
