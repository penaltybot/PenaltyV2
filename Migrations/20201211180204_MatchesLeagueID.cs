using Microsoft.EntityFrameworkCore.Migrations;

namespace PenaltyV2.Migrations
{
    public partial class MatchesLeagueID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeagueID",
                table: "Matches",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeagueID",
                table: "Matches");
        }
    }
}
