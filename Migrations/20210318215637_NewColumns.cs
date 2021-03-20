using Microsoft.EntityFrameworkCore.Migrations;

namespace PenaltyV2.Migrations
{
    public partial class NewColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LeagueID",
                table: "UsersComulativeScores",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeagueID",
                table: "UsersComulativeScores");
        }
    }
}
