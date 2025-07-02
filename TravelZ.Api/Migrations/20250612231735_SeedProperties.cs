using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TravelZ.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "Address", "AirConditioning", "Bathrooms", "Beds", "Country", "Description", "Name", "OwnerId", "Parking", "PetFriendly", "Pools", "TVs", "Wifi" },
                values: new object[,]
                {
                    { 1000, "123 Lake St", true, 2, 3, "USA", "A beautiful house by the lake.", "Lake House", null, true, true, 1, 2, true },
                    { 1001, "456 Mountain Rd", false, 1, 2, "USA", "Cozy cabin in the mountains.", "Mountain Cabin", null, true, false, 0, 1, false }
                });

            migrationBuilder.Sql("UPDATE sqlite_sequence SET seq = 1001 WHERE name = 'Properties';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1000);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1001);
        }
    }
}
