using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Shopping.Api.Migrations
{
    public partial class RefreshTokenAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "14e0a9db-8210-4ba8-a083-6a595f14f677");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44891df9-ba3b-4453-825c-7d6ec8fb79cc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "95ff21bf-13f8-4540-a430-08bfccdeb8d4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d7fbd497-97db-43d4-8aec-a3eabf6ebb9a");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "aca28dd8-d5a9-4cf3-ba10-7808ec336304", "14e5b7e6-1e62-49d0-b188-a4b4be530395", "Viewer", "VIEWER" },
                    { "1f9af506-0e6c-4f8f-a356-0b917c5247b3", "0aa5f0d3-bbe4-473a-b4ea-ea0fb0b34b05", "standardUser", "STANDARDUSER" },
                    { "a235488b-52ef-4bff-a223-77e28d2821d7", "e1fbf288-5d96-4052-a032-2b3e97b3d66a", "manager", "MANAGER" },
                    { "be7f5018-bd77-4ce5-a804-2972eabd0100", "a89cb13a-575d-492f-a925-af280d0fd863", "Administrator", "ADMINISTRATOR" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1f9af506-0e6c-4f8f-a356-0b917c5247b3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a235488b-52ef-4bff-a223-77e28d2821d7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aca28dd8-d5a9-4cf3-ba10-7808ec336304");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "be7f5018-bd77-4ce5-a804-2972eabd0100");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d7fbd497-97db-43d4-8aec-a3eabf6ebb9a", "445e96d4-0a50-4bca-8d30-417828667281", "Viewer", "VIEWER" },
                    { "14e0a9db-8210-4ba8-a083-6a595f14f677", "fe40a698-5da8-4cc4-8da4-d6eed3ffee5d", "standardUser", "STANDARDUSER" },
                    { "95ff21bf-13f8-4540-a430-08bfccdeb8d4", "0371afba-6a6b-40f4-93bb-855b431fd5eb", "manager", "MANAGER" },
                    { "44891df9-ba3b-4453-825c-7d6ec8fb79cc", "f392bc97-fcfe-4a4d-9c6d-341323851119", "Administrator", "ADMINISTRATOR" }
                });
        }
    }
}
