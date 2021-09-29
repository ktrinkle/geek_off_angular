using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace geek_off_angular.Migrations
{
    public partial class UserInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "player_num",
                table: "team_user",
                type: "integer",
                nullable: true);

            migrationBuilder.InsertData(
                table: "team_user",
                columns: new[] { "id", "admin_flag", "badge_id", "login_time", "player_num", "team_no", "username", "yevent" },
                values: new object[,]
                {
                    { 100L, true, "362525", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, "362525@corpaa.aa.com", "e21" },
                    { 101L, true, "446792", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, "446792@corpaa.aa.com", "e21" },
                    { 102L, true, "226250", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, "226250@corpaa.aa.com", "e21" },
                    { 103L, true, "288132", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, "288132@corpaa.aa.com", "e21" },
                    { 104L, true, "285557", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, "285557@corpaa.aa.com", "e21" },
                    { 105L, true, "274798", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, "274798@corpaa.aa.com", "e21" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 100L);

            migrationBuilder.DeleteData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 101L);

            migrationBuilder.DeleteData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 102L);

            migrationBuilder.DeleteData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 103L);

            migrationBuilder.DeleteData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 104L);

            migrationBuilder.DeleteData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 105L);

            migrationBuilder.DropColumn(
                name: "player_num",
                table: "team_user");
        }
    }
}
