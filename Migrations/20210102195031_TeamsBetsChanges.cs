using Microsoft.EntityFrameworkCore.Migrations;

namespace PenaltyV2.Migrations
{
    public partial class TeamsBetsChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TeamId",
                table: "Teams",
                newName: "TeamId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Teams",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Idmatch",
                table: "Bets",
                newName: "IdmatchAPI");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdmatchAPI",
                table: "Bets",
                newName: "Idmatch");
        }
    }
}
