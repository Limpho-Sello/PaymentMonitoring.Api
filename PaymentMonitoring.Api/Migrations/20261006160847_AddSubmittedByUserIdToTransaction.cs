using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentMonitoring.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddSubmittedByUserIdToTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubmittedByUserId",
                table: "Transactions",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubmittedByUserId",
                table: "Transactions");
        }
    }
}
