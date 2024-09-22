using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace geek_off_angular.Migrations
{
    /// <inheritdoc />
    public partial class FixInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "event_master",
                keyColumn: "yevent",
                keyValue: "e21");

            migrationBuilder.DeleteData(
                table: "question_ans",
                keyColumns: new[] { "question_num", "round_num", "yevent" },
                keyValues: new object[] { 201, 2, "e21" });

            migrationBuilder.DeleteData(
                table: "round_category",
                keyColumns: new[] { "round_num", "sub_category_num", "yevent" },
                keyValues: new object[] { 3, 10, "e21" });

            migrationBuilder.DeleteData(
                table: "round_category",
                keyColumns: new[] { "round_num", "sub_category_num", "yevent" },
                keyValues: new object[] { 3, 20, "e21" });

            migrationBuilder.DeleteData(
                table: "round_category",
                keyColumns: new[] { "round_num", "sub_category_num", "yevent" },
                keyValues: new object[] { 3, 30, "e21" });

            migrationBuilder.DeleteData(
                table: "round_category",
                keyColumns: new[] { "round_num", "sub_category_num", "yevent" },
                keyValues: new object[] { 3, 40, "e21" });

            migrationBuilder.DeleteData(
                table: "round_category",
                keyColumns: new[] { "round_num", "sub_category_num", "yevent" },
                keyValues: new object[] { 3, 50, "e21" });

            migrationBuilder.DeleteData(
                table: "scoreposs",
                keyColumns: new[] { "question_num", "round_num", "survey_order", "yevent" },
                keyValues: new object[] { 201, 2, 1, "e21" });

            migrationBuilder.DeleteData(
                table: "scoreposs",
                keyColumns: new[] { "question_num", "round_num", "survey_order", "yevent" },
                keyValues: new object[] { 201, 2, 2, "e21" });

            migrationBuilder.DeleteData(
                table: "scoreposs",
                keyColumns: new[] { "question_num", "round_num", "survey_order", "yevent" },
                keyValues: new object[] { 201, 2, 3, "e21" });

            migrationBuilder.DeleteData(
                table: "scoreposs",
                keyColumns: new[] { "question_num", "round_num", "survey_order", "yevent" },
                keyValues: new object[] { 201, 2, 4, "e21" });

            migrationBuilder.DeleteData(
                table: "scoreposs",
                keyColumns: new[] { "question_num", "round_num", "survey_order", "yevent" },
                keyValues: new object[] { 201, 2, 5, "e21" });

            migrationBuilder.DeleteData(
                table: "team_reference",
                keyColumns: new[] { "team_num", "yevent" },
                keyValues: new object[] { 1, "e21" });

            migrationBuilder.UpdateData(
                table: "admin_user",
                keyColumn: "id",
                keyValue: 100L,
                column: "username",
                value: "ktrinkle");

            migrationBuilder.UpdateData(
                table: "admin_user",
                keyColumn: "id",
                keyValue: 101L,
                column: "username",
                value: "krussell");

            migrationBuilder.UpdateData(
                table: "admin_user",
                keyColumn: "id",
                keyValue: 102L,
                column: "username",
                value: "damnral");

            migrationBuilder.UpdateData(
                table: "admin_user",
                keyColumn: "id",
                keyValue: 103L,
                column: "username",
                value: "dmullings");

            migrationBuilder.UpdateData(
                table: "event_master",
                keyColumn: "yevent",
                keyValue: "t24",
                column: "event_name",
                value: "Test 2024");

            migrationBuilder.InsertData(
                table: "question_ans",
                columns: new[] { "question_num", "round_num", "yevent", "correct_answer", "daily_double", "match_question", "media_file", "media_type", "multiple_choice", "text_answer", "text_answer2", "text_answer3", "text_answer4", "text_question" },
                values: new object[] { 201, 2, "t24", null, false, null, null, null, null, null, null, null, null, "Name your favorite developer." });

            migrationBuilder.InsertData(
                table: "round_category",
                columns: new[] { "round_num", "sub_category_num", "yevent", "category_name", "id" },
                values: new object[,]
                {
                    { 3, 10, "t24", "Potent Potables", 1 },
                    { 3, 20, "t24", "Therapists", 2 },
                    { 3, 30, "t24", "An Album", 3 },
                    { 3, 40, "t24", "Potpourri", 4 },
                    { 3, 50, "t24", "Words that begin with G", 5 }
                });

            migrationBuilder.InsertData(
                table: "scoreposs",
                columns: new[] { "question_num", "round_num", "survey_order", "yevent", "ptsposs", "question_answer" },
                values: new object[,]
                {
                    { 201, 2, 1, "t24", 8, "Kevin" },
                    { 201, 2, 2, "t24", 7, "Kristin" },
                    { 201, 2, 3, "t24", 6, "Diyalo" },
                    { 201, 2, 4, "t24", 5, "Dan" },
                    { 201, 2, 5, "t24", 2, "Li" }
                });

            migrationBuilder.InsertData(
                table: "team_reference",
                columns: new[] { "team_num", "yevent", "dollarraised", "login_time", "team_guid", "teamname" },
                values: new object[,]
                {
                    { 1, "t24", 1000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), "Go Aggies" },
                    { 2, "t24", 50m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), "Go Planes" }
                });

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 104L,
                column: "yevent",
                value: "t24");

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 106L,
                column: "yevent",
                value: "t24");

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 107L,
                column: "yevent",
                value: "t24");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "question_ans",
                keyColumns: new[] { "question_num", "round_num", "yevent" },
                keyValues: new object[] { 201, 2, "t24" });

            migrationBuilder.DeleteData(
                table: "round_category",
                keyColumns: new[] { "round_num", "sub_category_num", "yevent" },
                keyValues: new object[] { 3, 10, "t24" });

            migrationBuilder.DeleteData(
                table: "round_category",
                keyColumns: new[] { "round_num", "sub_category_num", "yevent" },
                keyValues: new object[] { 3, 20, "t24" });

            migrationBuilder.DeleteData(
                table: "round_category",
                keyColumns: new[] { "round_num", "sub_category_num", "yevent" },
                keyValues: new object[] { 3, 30, "t24" });

            migrationBuilder.DeleteData(
                table: "round_category",
                keyColumns: new[] { "round_num", "sub_category_num", "yevent" },
                keyValues: new object[] { 3, 40, "t24" });

            migrationBuilder.DeleteData(
                table: "round_category",
                keyColumns: new[] { "round_num", "sub_category_num", "yevent" },
                keyValues: new object[] { 3, 50, "t24" });

            migrationBuilder.DeleteData(
                table: "scoreposs",
                keyColumns: new[] { "question_num", "round_num", "survey_order", "yevent" },
                keyValues: new object[] { 201, 2, 1, "t24" });

            migrationBuilder.DeleteData(
                table: "scoreposs",
                keyColumns: new[] { "question_num", "round_num", "survey_order", "yevent" },
                keyValues: new object[] { 201, 2, 2, "t24" });

            migrationBuilder.DeleteData(
                table: "scoreposs",
                keyColumns: new[] { "question_num", "round_num", "survey_order", "yevent" },
                keyValues: new object[] { 201, 2, 3, "t24" });

            migrationBuilder.DeleteData(
                table: "scoreposs",
                keyColumns: new[] { "question_num", "round_num", "survey_order", "yevent" },
                keyValues: new object[] { 201, 2, 4, "t24" });

            migrationBuilder.DeleteData(
                table: "scoreposs",
                keyColumns: new[] { "question_num", "round_num", "survey_order", "yevent" },
                keyValues: new object[] { 201, 2, 5, "t24" });

            migrationBuilder.DeleteData(
                table: "team_reference",
                keyColumns: new[] { "team_num", "yevent" },
                keyValues: new object[] { 1, "t24" });

            migrationBuilder.DeleteData(
                table: "team_reference",
                keyColumns: new[] { "team_num", "yevent" },
                keyValues: new object[] { 2, "t24" });

            migrationBuilder.UpdateData(
                table: "admin_user",
                keyColumn: "id",
                keyValue: 100L,
                column: "username",
                value: "362525");

            migrationBuilder.UpdateData(
                table: "admin_user",
                keyColumn: "id",
                keyValue: 101L,
                column: "username",
                value: "446792");

            migrationBuilder.UpdateData(
                table: "admin_user",
                keyColumn: "id",
                keyValue: 102L,
                column: "username",
                value: "226250");

            migrationBuilder.UpdateData(
                table: "admin_user",
                keyColumn: "id",
                keyValue: 103L,
                column: "username",
                value: "288132");

            migrationBuilder.UpdateData(
                table: "event_master",
                keyColumn: "yevent",
                keyValue: "t24",
                column: "event_name",
                value: "Employee 2021");

            migrationBuilder.InsertData(
                table: "event_master",
                columns: new[] { "yevent", "event_name", "round2_control", "round3_control", "sel_event" },
                values: new object[] { "e21", "Employee 2021", 0, 3, true });

            migrationBuilder.InsertData(
                table: "question_ans",
                columns: new[] { "question_num", "round_num", "yevent", "correct_answer", "daily_double", "match_question", "media_file", "media_type", "multiple_choice", "text_answer", "text_answer2", "text_answer3", "text_answer4", "text_question" },
                values: new object[] { 201, 2, "e21", null, false, null, null, null, null, null, null, null, null, "Name your favorite developer." });

            migrationBuilder.InsertData(
                table: "round_category",
                columns: new[] { "round_num", "sub_category_num", "yevent", "category_name", "id" },
                values: new object[,]
                {
                    { 3, 10, "e21", "Potent Potables", 1 },
                    { 3, 20, "e21", "Therapists", 2 },
                    { 3, 30, "e21", "An Album", 3 },
                    { 3, 40, "e21", "Potpourri", 4 },
                    { 3, 50, "e21", "Words that begin with G", 5 }
                });

            migrationBuilder.InsertData(
                table: "scoreposs",
                columns: new[] { "question_num", "round_num", "survey_order", "yevent", "ptsposs", "question_answer" },
                values: new object[,]
                {
                    { 201, 2, 1, "e21", 8, "Kevin" },
                    { 201, 2, 2, "e21", 7, "Kristin" },
                    { 201, 2, 3, "e21", 6, "Diyalo" },
                    { 201, 2, 4, "e21", 5, "Dan" },
                    { 201, 2, 5, "e21", 2, "Li" }
                });

            migrationBuilder.InsertData(
                table: "team_reference",
                columns: new[] { "team_num", "yevent", "dollarraised", "login_time", "team_guid", "teamname" },
                values: new object[] { 1, "e21", 1000m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), "Go Aggies" });

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 104L,
                column: "yevent",
                value: "e21");

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 106L,
                column: "yevent",
                value: "e21");

            migrationBuilder.UpdateData(
                table: "team_user",
                keyColumn: "id",
                keyValue: 107L,
                column: "yevent",
                value: "e21");
        }
    }
}
