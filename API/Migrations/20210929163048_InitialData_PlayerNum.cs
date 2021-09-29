using Microsoft.EntityFrameworkCore.Migrations;

namespace geek_off_angular.Migrations
{
    public partial class InitialData_PlayerNum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 104L,
                column: "player_num",
                value: 1);

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 106L,
                column: "player_num",
                value: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 104L,
                column: "player_num",
                value: null);

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 106L,
                column: "player_num",
                value: null);
        }
    }
}
