using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AllupVol2.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnInProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DisCountPercentage",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<decimal>(
                name: "DisCountPrice",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisCountPercentage",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DisCountPrice",
                table: "Products");
        }
    }
}
