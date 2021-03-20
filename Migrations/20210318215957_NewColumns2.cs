using Microsoft.EntityFrameworkCore.Migrations;

namespace PenaltyV2.Migrations
{
    public partial class NewColumns2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TeamID",
                table: "TeamsStandings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamID",
                table: "TeamsStandings");
        }
    }
}
