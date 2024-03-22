using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShoeStudio.Migrations
{
    /// <inheritdoc />
    public partial class AddListNameAndDateToWishList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Wishlists",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ListName",
                table: "Wishlists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Wishlists");

            migrationBuilder.DropColumn(
                name: "ListName",
                table: "Wishlists");
        }
    }
}
