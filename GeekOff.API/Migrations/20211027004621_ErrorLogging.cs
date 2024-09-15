using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace geek_off_angular.Migrations
{
    public partial class ErrorLogging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "log_error",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    error_message = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_log_error", x => x.id);
                });

            migrationBuilder.UpdateData(
                table: "scoreposs",
                keyColumns: new[] { "question_no", "round_no", "survey_order", "yevent" },
                keyValues: new object[] { 201, 2, 4, "e21" },
                columns: new[] { "ptsposs", "question_answer" },
                values: new object[] { 5, "Dan" });

            migrationBuilder.UpdateData(
                table: "scoreposs",
                keyColumns: new[] { "question_no", "round_no", "survey_order", "yevent" },
                keyValues: new object[] { 201, 2, 5, "e21" },
                columns: new[] { "ptsposs", "question_answer" },
                values: new object[] { 2, "Li" });

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 100L,
                columns: new[] { "badge_id", "player_name" },
                values: new object[] { null, "Kevin Trinkle" });

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 101L,
                columns: new[] { "badge_id", "player_name" },
                values: new object[] { null, "Kristin Russell" });

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 102L,
                columns: new[] { "badge_id", "player_name" },
                values: new object[] { null, "Diyalo Manral" });

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 103L,
                columns: new[] { "badge_id", "player_name" },
                values: new object[] { null, "Dan Mullings" });

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 105L,
                columns: new[] { "badge_id", "player_name", "username" },
                values: new object[] { null, "Jay Cox", "jay.cox_aa.com#EXT#@geekoff.onmicrosoft.com" });

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 106L,
                column: "badge_id",
                value: null);

            migrationBuilder.InsertData(
                table: "team_user",
                columns: new[] { "id", "admin_flag", "badge_id", "login_time", "player_name", "player_num", "team_no", "username", "workgroup_name", "yevent" },
                values: new object[,]
                {
                    { 107L, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Roger Marsolek", 1, 2, "991023@geekoff.onmicrosoft.com", "Information Technology", "e21" },
                    { 108L, true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kevin Trinkle", 0, 0, "kevin.trinkle_aa.com#EXT#@geekoff.onmicrosoft.com", "Information Technology", "e21" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "log_error");

            migrationBuilder.DeleteData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 107L);

            migrationBuilder.DeleteData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 108L);

            migrationBuilder.UpdateData(
                table: "scoreposs",
                keyColumns: new[] { "question_no", "round_no", "survey_order", "yevent" },
                keyValues: new object[] { 201, 2, 4, "e21" },
                columns: new[] { "ptsposs", "question_answer" },
                values: new object[] { 6, "Li" });

            migrationBuilder.UpdateData(
                table: "scoreposs",
                keyColumns: new[] { "question_no", "round_no", "survey_order", "yevent" },
                keyValues: new object[] { 201, 2, 5, "e21" },
                columns: new[] { "ptsposs", "question_answer" },
                values: new object[] { 5, "Dan" });

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 100L,
                columns: new[] { "badge_id", "player_name" },
                values: new object[] { "362525", null });

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 101L,
                columns: new[] { "badge_id", "player_name" },
                values: new object[] { "446792", null });

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 102L,
                columns: new[] { "badge_id", "player_name" },
                values: new object[] { "226250", null });

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 103L,
                columns: new[] { "badge_id", "player_name" },
                values: new object[] { "288132", null });

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 105L,
                columns: new[] { "badge_id", "player_name", "username" },
                values: new object[] { "274798", null, "274798@geekoff.onmicrosoft.com" });

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 106L,
                column: "badge_id",
                value: "641903");
        }
    }
}
