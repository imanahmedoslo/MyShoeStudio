using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShoeStudio.Migrations
{
    /// <inheritdoc />
    public partial class AddUnqueConstraintOnWishListNmaePerUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ListName",
                table: "Wishlists",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_ListName_Id",
                table: "Wishlists",
                columns: new[] { "ListName", "Id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Wishlists_ListName_Id",
                table: "Wishlists");

            migrationBuilder.AlterColumn<string>(
                name: "ListName",
                table: "Wishlists",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
