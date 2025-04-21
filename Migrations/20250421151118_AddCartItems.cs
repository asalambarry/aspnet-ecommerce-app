using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopZone.Migrations
{
    /// <inheritdoc />
    public partial class AddCartItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "CartItems",
                newName: "DateCreated");

            migrationBuilder.AddColumn<string>(
                name: "CartId",
                table: "CartItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CartId",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "CartItems",
                newName: "UserId");
        }
    }
}
