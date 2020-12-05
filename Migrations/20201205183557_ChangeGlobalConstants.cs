using Microsoft.EntityFrameworkCore.Migrations;

namespace PenaltyV2.Migrations
{
    public partial class ChangeGlobalConstants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currentmatchday",
                table: "Globalconstants");

            migrationBuilder.RenameColumn(
                name: "Currentyear",
                table: "Globalconstants",
                newName: "Variable");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "Globalconstants",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "Globalconstants");

            migrationBuilder.RenameColumn(
                name: "Variable",
                table: "Globalconstants",
                newName: "Currentyear");

            migrationBuilder.AddColumn<int>(
                name: "Currentmatchday",
                table: "Globalconstants",
                nullable: true);
        }
    }
}
