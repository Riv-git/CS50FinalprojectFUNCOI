using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Fundacionproj.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "517323de-84a0-4ef7-8384-3fc776561546", null, "admin", "admin" },
                    { "8f97dc9d-0abe-4132-a9fc-74b7b58aebb2", null, "seller", "seller" },
                    { "eb1d813a-56cc-4af9-b92b-9e4b622d0837", null, "client", "client" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "517323de-84a0-4ef7-8384-3fc776561546");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8f97dc9d-0abe-4132-a9fc-74b7b58aebb2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eb1d813a-56cc-4af9-b92b-9e4b622d0837");
        }
    }
}
