using Microsoft.EntityFrameworkCore.Migrations;

namespace PenaltyV2.Migrations
{
    public partial class MatchesOdds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Oddsaway",
                table: "Matches",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Oddshome",
                table: "Matches",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Oddsaway",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Oddshome",
                table: "Matches");
        }
    }
}
