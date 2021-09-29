using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace geek_off_angular.Migrations
{
    public partial class InitialData_Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "player_name",
                table: "team_user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "workgroup_name",
                table: "team_user",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 100L,
                column: "username",
                value: "362525@geekoff.onmicrosoft.com");

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 101L,
                column: "username",
                value: "446792@geekoff.onmicrosoft.com");

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 102L,
                column: "username",
                value: "226250@geekoff.onmicrosoft.com");

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 103L,
                column: "username",
                value: "288132@geekoff.onmicrosoft.com");

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 104L,
                columns: new[] { "player_name", "team_no", "username", "workgroup_name" },
                values: new object[] { "Grant Hill", 1, "285557@geekoff.onmicrosoft.com", "Information Technology" });

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 105L,
                column: "username",
                value: "274798@geekoff.onmicrosoft.com");

            migrationBuilder.InsertData(
                table: "team_user",
                columns: new[] { "id", "admin_flag", "badge_id", "login_time", "player_name", "player_num", "team_no", "username", "workgroup_name", "yevent" },
                values: new object[] { 106L, false, "641903", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Brandon Heath", null, 1, "641903@geekoff.onmicrosoft.com", "Information Technology", "e21" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 106L);

            migrationBuilder.DropColumn(
                name: "player_name",
                table: "team_user");

            migrationBuilder.DropColumn(
                name: "workgroup_name",
                table: "team_user");

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 100L,
                column: "username",
                value: "362525@corpaa.aa.com");

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 101L,
                column: "username",
                value: "446792@corpaa.aa.com");

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 102L,
                column: "username",
                value: "226250@corpaa.aa.com");

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 103L,
                column: "username",
                value: "288132@corpaa.aa.com");

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 104L,
                columns: new[] { "team_no", "username" },
                values: new object[] { 0, "285557@corpaa.aa.com" });

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 105L,
                column: "username",
                value: "274798@corpaa.aa.com");
        }
    }
}
