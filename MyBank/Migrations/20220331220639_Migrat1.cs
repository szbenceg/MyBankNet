using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBank.Migrations
{
    public partial class Migrat1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DestinationAccountOwnerName",
                table: "Transactions",
                newName: "BenificaryName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BenificaryName",
                table: "Transactions",
                newName: "DestinationAccountOwnerName");
        }
    }
}
