using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentMonitoring.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddSubmittedAtToTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedAt",
                table: "Transactions",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubmittedAt",
                table: "Transactions");
        }
    }
}
