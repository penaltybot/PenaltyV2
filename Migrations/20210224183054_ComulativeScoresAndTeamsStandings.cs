using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PenaltyV2.Migrations
{
    public partial class ComulativeScoresAndTeamsStandings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TeamsStandings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Rank = table.Column<int>(nullable: true),
                    Points = table.Column<int>(nullable: true),
                    Form = table.Column<string>(nullable: true),
                    MatchesPlayed = table.Column<int>(nullable: true),
                    Wins = table.Column<int>(nullable: true),
                    Draws = table.Column<int>(nullable: true),
                    Losses = table.Column<int>(nullable: true),
                    GoalsFor = table.Column<int>(nullable: true),
                    GoalsAgainst = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamsStandings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersComulativeScores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(nullable: true),
                    Matchday = table.Column<string>(nullable: true),
                    Score = table.Column<decimal>(nullable: true),
                    CorrectPredictions = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersComulativeScores", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamsStandings");

            migrationBuilder.DropTable(
                name: "UsersComulativeScores");
        }
    }
}
