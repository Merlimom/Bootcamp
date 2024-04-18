using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MovementMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movements_Accounts_AccountId",
                table: "Movements");

            migrationBuilder.DropIndex(
                name: "IX_Movements_AccountId",
                table: "Movements");

            migrationBuilder.RenameColumn(
                name: "Destination",
                table: "Movements",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Movements",
                newName: "MovementType");

            migrationBuilder.AddColumn<int>(
                name: "AccountDestinationId",
                table: "Movements",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccountSourceId",
                table: "Movements",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Movements_AccountSourceId",
                table: "Movements",
                column: "AccountSourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movements_Accounts_AccountSourceId",
                table: "Movements",
                column: "AccountSourceId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movements_Accounts_AccountSourceId",
                table: "Movements");

            migrationBuilder.DropIndex(
                name: "IX_Movements_AccountSourceId",
                table: "Movements");

            migrationBuilder.DropColumn(
                name: "AccountDestinationId",
                table: "Movements");

            migrationBuilder.DropColumn(
                name: "AccountSourceId",
                table: "Movements");

            migrationBuilder.RenameColumn(
                name: "MovementType",
                table: "Movements",
                newName: "AccountId");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Movements",
                newName: "Destination");

            migrationBuilder.CreateIndex(
                name: "IX_Movements_AccountId",
                table: "Movements",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movements_Accounts_AccountId",
                table: "Movements",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
