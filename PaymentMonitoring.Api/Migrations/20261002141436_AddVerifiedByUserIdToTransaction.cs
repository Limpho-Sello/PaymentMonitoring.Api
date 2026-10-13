using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentMonitoring.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddVerifiedByUserIdToTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VerifiedByUserId",
                table: "Transactions",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerifiedByUserId",
                table: "Transactions");
        }
    }
}
