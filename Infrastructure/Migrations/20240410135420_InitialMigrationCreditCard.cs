using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrationCreditCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreditCard_Currencies_CurrencyId",
                table: "CreditCard");

            migrationBuilder.DropForeignKey(
                name: "FK_CreditCard_Customers_CustomerId",
                table: "CreditCard");

            migrationBuilder.RenameTable(
                name: "CreditCard",
                newName: "CreditCards");

            migrationBuilder.RenameIndex(
                name: "IX_CreditCard_CustomerId",
                table: "CreditCards",
                newName: "IX_CreditCards_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_CreditCard_CurrencyId",
                table: "CreditCards",
                newName: "IX_CreditCards_CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CreditCards_Currencies_CurrencyId",
                table: "CreditCards",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreditCards_Customers_CustomerId",
                table: "CreditCards",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreditCards_Currencies_CurrencyId",
                table: "CreditCards");

            migrationBuilder.DropForeignKey(
                name: "FK_CreditCards_Customers_CustomerId",
                table: "CreditCards");

            migrationBuilder.RenameTable(
                name: "CreditCards",
                newName: "CreditCard");

            migrationBuilder.RenameIndex(
                name: "IX_CreditCards_CustomerId",
                table: "CreditCard",
                newName: "IX_CreditCard_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_CreditCards_CurrencyId",
                table: "CreditCard",
                newName: "IX_CreditCard_CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CreditCard_Currencies_CurrencyId",
                table: "CreditCard",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreditCard_Customers_CustomerId",
                table: "CreditCard",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
