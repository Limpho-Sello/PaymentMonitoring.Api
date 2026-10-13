using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentMonitoring.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionAuditRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SubmittedByUserId",
                table: "Transactions",
                column: "SubmittedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_VerifiedByUserId",
                table: "Transactions",
                column: "VerifiedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_SubmittedByUserId",
                table: "Transactions",
                column: "SubmittedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_VerifiedByUserId",
                table: "Transactions",
                column: "VerifiedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_SubmittedByUserId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_VerifiedByUserId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_SubmittedByUserId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_VerifiedByUserId",
                table: "Transactions");
        }
    }
}
