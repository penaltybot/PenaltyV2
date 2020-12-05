using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PenaltyV2.Migrations
{
    public partial class MatchesModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Matchday = table.Column<int>(nullable: true),
                    Matchnumber = table.Column<int>(nullable: true),
                    Hometeam = table.Column<string>(nullable: true),
                    Hometeamgoals = table.Column<int>(nullable: true),
                    awayteam = table.Column<string>(nullable: true),
                    awayteamgoals = table.Column<int>(nullable: true),
                    idawayteam = table.Column<int>(nullable: true),
                    idhometeam = table.Column<int>(nullable: true),
                    status = table.Column<string>(nullable: true),
                    competitionyear = table.Column<string>(nullable: true),
                    utcDate = table.Column<DateTime>(nullable: true),
                    idmatchAPI = table.Column<int>(nullable: true),
                    result1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Matches");
        }
    }
}
