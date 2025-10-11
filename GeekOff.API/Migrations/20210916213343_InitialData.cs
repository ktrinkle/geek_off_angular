using Microsoft.EntityFrameworkCore.Migrations;

namespace geek_off_angular.Migrations
{
    public partial class InitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "event_master",
                columns: new[] { "yevent", "event_name", "sel_event" },
                values: new object[,]
                {
                    { "e21", "Employee 2021", true },
                    { "e19", "Employee 2019", null }
                });

            migrationBuilder.InsertData(
                table: "question_ans",
                columns: new[] { "question_no", "round_no", "yevent", "multiple_choice", "text_answer", "text_question", "wrong_answer1", "wrong_answer2", "wrong_answer3" },
                values: new object[] { 201, 2, "e21", null, null, "Name your favorite developer.", null, null, null });

            migrationBuilder.InsertData(
                table: "scoreposs",
                columns: new[] { "question_no", "round_no", "survey_order", "yevent", "ptsposs", "question_answer" },
                values: new object[,]
                {
                    { 201, 2, 1, "e21", 8, "Kevin" },
                    { 201, 2, 2, "e21", 7, "Kristin" },
                    { 201, 2, 3, "e21", 6, "Diyalo" },
                    { 201, 2, 4, "e21", 6, "Li" },
                    { 201, 2, 5, "e21", 5, "Dan" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "event_master",
                keyColumn: "yevent",
                keyValue: "e19");

            migrationBuilder.DeleteData(
                table: "event_master",
                keyColumn: "yevent",
                keyValue: "e21");

            migrationBuilder.DeleteData(
                table: "question_ans",
                keyColumns: new[] { "question_no", "round_no", "yevent" },
                keyValues: new object[] { 201, 2, "e21" });

            migrationBuilder.DeleteData(
                table: "scoreposs",
                keyColumns: new[] { "question_no", "round_no", "survey_order", "yevent" },
                keyValues: new object[] { 201, 2, 1, "e21" });

            migrationBuilder.DeleteData(
                table: "scoreposs",
                keyColumns: new[] { "question_no", "round_no", "survey_order", "yevent" },
                keyValues: new object[] { 201, 2, 2, "e21" });

            migrationBuilder.DeleteData(
                table: "scoreposs",
                keyColumns: new[] { "question_no", "round_no", "survey_order", "yevent" },
                keyValues: new object[] { 201, 2, 3, "e21" });

            migrationBuilder.DeleteData(
                table: "scoreposs",
                keyColumns: new[] { "question_no", "round_no", "survey_order", "yevent" },
                keyValues: new object[] { 201, 2, 4, "e21" });

            migrationBuilder.DeleteData(
                table: "scoreposs",
                keyColumns: new[] { "question_no", "round_no", "survey_order", "yevent" },
                keyValues: new object[] { 201, 2, 5, "e21" });
        }
    }
}
