using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PathToWealthAPI.Migrations
{
    /// <inheritdoc />
    public partial class migration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserFinancialData_UserId",
                table: "UserFinancialData",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFinancialData_User_UserId",
                table: "UserFinancialData",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFinancialData_User_UserId",
                table: "UserFinancialData");

            migrationBuilder.DropIndex(
                name: "IX_UserFinancialData_UserId",
                table: "UserFinancialData");
        }
    }
}
