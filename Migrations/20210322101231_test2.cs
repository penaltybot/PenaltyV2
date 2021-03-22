using Microsoft.EntityFrameworkCore.Migrations;

namespace PenaltyV2.Migrations
{
    public partial class test2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersComulativeScores",
                table: "UsersComulativeScores");

            migrationBuilder.RenameTable(
                name: "UsersComulativeScores",
                newName: "UsersCumulativeScores");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersCumulativeScores",
                table: "UsersCumulativeScores",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersCumulativeScores",
                table: "UsersCumulativeScores");

            migrationBuilder.RenameTable(
                name: "UsersCumulativeScores",
                newName: "UsersComulativeScores");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersComulativeScores",
                table: "UsersComulativeScores",
                column: "Id");
        }
    }
}
