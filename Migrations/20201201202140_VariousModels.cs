using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PenaltyV2.Migrations
{
    public partial class VariousModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "utcDate",
                table: "Matches",
                newName: "UtcDate");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Matches",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "result1",
                table: "Matches",
                newName: "Result1");

            migrationBuilder.RenameColumn(
                name: "idmatchAPI",
                table: "Matches",
                newName: "IdmatchAPI");

            migrationBuilder.RenameColumn(
                name: "idhometeam",
                table: "Matches",
                newName: "Idhometeam");

            migrationBuilder.RenameColumn(
                name: "idawayteam",
                table: "Matches",
                newName: "Idawayteam");

            migrationBuilder.RenameColumn(
                name: "competitionyear",
                table: "Matches",
                newName: "Competitionyear");

            migrationBuilder.RenameColumn(
                name: "awayteamgoals",
                table: "Matches",
                newName: "Awayteamgoals");

            migrationBuilder.RenameColumn(
                name: "awayteam",
                table: "Matches",
                newName: "Awayteam");

            migrationBuilder.CreateTable(
                name: "Bets",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Idmatch = table.Column<int>(nullable: false),
                    GoalsHomeTeam = table.Column<int>(nullable: true),
                    GoalsAwayTeam = table.Column<int>(nullable: true),
                    Result = table.Column<string>(nullable: true),
                    Score = table.Column<decimal>(nullable: true),
                    Perfect = table.Column<int>(nullable: true),
                    Matchday = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Globalconstants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Currentmatchday = table.Column<int>(nullable: true),
                    Currentyear = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Globalconstants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usersinfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Leagues = table.Column<string>(nullable: true),
                    Favoriteteam = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usersinfo", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bets");

            migrationBuilder.DropTable(
                name: "Globalconstants");

            migrationBuilder.DropTable(
                name: "Usersinfo");

            migrationBuilder.RenameColumn(
                name: "UtcDate",
                table: "Matches",
                newName: "utcDate");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Matches",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Result1",
                table: "Matches",
                newName: "result1");

            migrationBuilder.RenameColumn(
                name: "IdmatchAPI",
                table: "Matches",
                newName: "idmatchAPI");

            migrationBuilder.RenameColumn(
                name: "Idhometeam",
                table: "Matches",
                newName: "idhometeam");

            migrationBuilder.RenameColumn(
                name: "Idawayteam",
                table: "Matches",
                newName: "idawayteam");

            migrationBuilder.RenameColumn(
                name: "Competitionyear",
                table: "Matches",
                newName: "competitionyear");

            migrationBuilder.RenameColumn(
                name: "Awayteamgoals",
                table: "Matches",
                newName: "awayteamgoals");

            migrationBuilder.RenameColumn(
                name: "Awayteam",
                table: "Matches",
                newName: "awayteam");
        }
    }
}
