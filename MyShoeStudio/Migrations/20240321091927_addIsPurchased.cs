using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShoeStudio.Migrations
{
    /// <inheritdoc />
    public partial class addIsPurchased : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPurchased",
                table: "ShoppingLists",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPurchased",
                table: "ShoppingLists");
        }
    }
}
