using Microsoft.EntityFrameworkCore.Migrations;

namespace PenaltyV2.Migrations
{
    public partial class SendEmailReminder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SendEmailReminder",
                table: "Usersinfo",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendEmailReminder",
                table: "Usersinfo");
        }
    }
}
