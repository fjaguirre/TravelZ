using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelZ.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: false),
                    Beds = table.Column<int>(type: "INTEGER", nullable: false),
                    Bathrooms = table.Column<int>(type: "INTEGER", nullable: false),
                    TVs = table.Column<int>(type: "INTEGER", nullable: false),
                    Pools = table.Column<int>(type: "INTEGER", nullable: false),
                    PetFriendly = table.Column<bool>(type: "INTEGER", nullable: false),
                    Wifi = table.Column<bool>(type: "INTEGER", nullable: false),
                    Parking = table.Column<bool>(type: "INTEGER", nullable: false),
                    AirConditioning = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Properties");
        }
    }
}
