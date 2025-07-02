using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelZ.Api.Migrations
{
    /// <inheritdoc />
    public partial class MakeOwnerIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Properties",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_OwnerId",
                table: "Properties",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_AspNetUsers_OwnerId",
                table: "Properties",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_AspNetUsers_OwnerId",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_OwnerId",
                table: "Properties");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Properties",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
