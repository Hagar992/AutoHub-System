using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoHub_System.Migrations
{
    /// <inheritdoc />
    public partial class updtaeCarDoha : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Orders",
                newName: "Staetus");

            migrationBuilder.RenameColumn(
                name: "PriceWhenBook",
                table: "Orders",
                newName: "PricWhenBook");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Cars",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Staetus",
                table: "Orders",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "PricWhenBook",
                table: "Orders",
                newName: "PriceWhenBook");

            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "Cars",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
