using Microsoft.EntityFrameworkCore.Migrations;

namespace Shopping.Api.Migrations
{
    public partial class FreshStart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "167a155d-b9bd-4db8-b51a-44f6eae4c096");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "43e866fe-cc65-4a24-8e5d-3d211df4599a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "46bdb580-34f9-4473-8c55-209ec10a92ca");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e67df5b7-87cd-452b-a059-e0ad0371b9a5");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                maxLength: 75,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(135)",
                oldMaxLength: 75);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(135)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebApiAdminInfoOnly",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WebApiAdminInfoOnly",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "nvarchar(135)",
                maxLength: 75,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 75);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(135)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "43e866fe-cc65-4a24-8e5d-3d211df4599a", "8fe27cd3-ca5e-41af-b78d-0bc776366484", "Viewer", "VIEWER" },
                    { "167a155d-b9bd-4db8-b51a-44f6eae4c096", "9eb25b10-8b85-461d-a228-3b311934a2cf", "standardUser", "STANDARDUSER" },
                    { "46bdb580-34f9-4473-8c55-209ec10a92ca", "a38f224c-ca69-480a-bfe3-0ec55e4c6134", "manager", "MANAGER" },
                    { "e67df5b7-87cd-452b-a059-e0ad0371b9a5", "8a2dce55-9178-41d1-97ee-6d2465c63950", "Administrator", "ADMINISTRATOR" }
                });
        }
    }
}
