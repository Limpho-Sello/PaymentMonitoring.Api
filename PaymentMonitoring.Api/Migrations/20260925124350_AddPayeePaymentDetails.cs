using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentMonitoring.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddPayeePaymentDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PayeeAccountNumber",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SwiftCode",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayeeAccountNumber",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SwiftCode",
                table: "Transactions");
        }
    }
}
