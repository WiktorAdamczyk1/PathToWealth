using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PathToWealthAPI.Migrations
{
    /// <inheritdoc />
    public partial class InflationAndWithdrawalRates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "InflationRate",
                table: "UserFinancialData",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WithdrawalRate",
                table: "UserFinancialData",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InflationRate",
                table: "UserFinancialData");

            migrationBuilder.DropColumn(
                name: "WithdrawalRate",
                table: "UserFinancialData");
        }
    }
}
