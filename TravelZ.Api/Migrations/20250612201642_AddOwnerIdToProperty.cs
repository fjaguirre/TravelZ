﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelZ.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerIdToProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Properties",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Properties");
        }
    }
}
