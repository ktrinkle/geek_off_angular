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
                    { 100L, true, "000001", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, "ktri@geekoff.onmicrosoft.com", "e21" },
                    { 101L, true, "000002", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, "kris@geekoff.onmicrosoft.com", "e21" },
                    { 102L, true, "000004", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, "dman@geekoff.onmicrosoft.com", "e21" },
                    { 103L, true, "000003", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, "dnol@geekoff.onmicrosoft.com", "e21" },
                    { 104L, true, "000006", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, "bhea@geekoff.onmicrosoft.com", "e21" },
                    { 105L, true, "000005", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, "jayc@geekoff.onmicrosoft.com", "e21" }
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
