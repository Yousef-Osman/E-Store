using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Store.Data.Migrations
{
    /// <inheritdoc />
    public partial class addCreationAndLastModifiedDatesInCartItemsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "CartItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "CartItems",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "CartItems");
        }
    }
}
