using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiStarter.Migrations
{
    public partial class added_some_sample_data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2e2045dd-2a16-461d-8662-74ab618a232b", "1ddac2b2-63e7-4c26-b87b-dbd4d0c6361b", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "db85aa44-5efc-41e7-837f-a46c10a8cfe6", "1de143f5-da83-41e0-b202-5aaa6bd71b3d", "Manager", "MANAGER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2e2045dd-2a16-461d-8662-74ab618a232b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db85aa44-5efc-41e7-837f-a46c10a8cfe6");
        }
    }
}
