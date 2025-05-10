using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PantryDjango.Migrations
{
    /// <inheritdoc />
    public partial class AddBarcodeToFoodItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "FoodItems",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "FoodItems");
        }
    }
}
