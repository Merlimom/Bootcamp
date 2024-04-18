using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RequestMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "UserRequests");

            migrationBuilder.DropColumn(
                name: "CreditCardBrand",
                table: "UserRequests");

            migrationBuilder.DropColumn(
                name: "PaymentTerm",
                table: "UserRequests");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "UserRequests",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "UserRequests");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "UserRequests",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CreditCardBrand",
                table: "UserRequests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentTerm",
                table: "UserRequests",
                type: "integer",
                nullable: true);
        }
    }
}
