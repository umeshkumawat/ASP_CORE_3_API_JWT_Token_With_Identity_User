using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityWithJWT.Migrations
{
    public partial class AddRolesToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "67a15c25-7a5d-41bb-8f7d-961246200f6a", "5dc6fba5-882d-4902-8d2f-63db578ee8a2", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "64463c8a-5099-4ad7-a1e0-76f603df6000", "47988e80-e02f-4dec-96b9-97413f44031e", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "64463c8a-5099-4ad7-a1e0-76f603df6000");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "67a15c25-7a5d-41bb-8f7d-961246200f6a");
        }
    }
}
