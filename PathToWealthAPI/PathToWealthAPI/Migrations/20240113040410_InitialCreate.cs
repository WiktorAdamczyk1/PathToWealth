using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PathToWealthAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "UserFinancialData",
                columns: table => new
                {
                    DataId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    InitialInvestment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartInvestementYear = table.Column<int>(type: "int", nullable: false),
                    StartWithdrawalYear = table.Column<int>(type: "int", nullable: false),
                    IsInvestmentMonthly = table.Column<bool>(type: "bit", nullable: false),
                    YearlyOrMonthlySavings = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockAnnualReturn = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockCostRatio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BondAnnualReturn = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BondCostRatio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockToBondRatio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RetirementDuration = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFinancialData", x => x.DataId);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    RefreshTokenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Revoked = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.RefreshTokenId);
                    table.ForeignKey(
                        name: "FK_RefreshToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "UserFinancialData");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
