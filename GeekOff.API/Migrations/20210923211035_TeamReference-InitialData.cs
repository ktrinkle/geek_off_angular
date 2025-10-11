using Microsoft.EntityFrameworkCore.Migrations;

namespace geek_off_angular.Migrations
{
    public partial class TeamReferenceInitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "wrong_answer3",
                table: "question_ans",
                newName: "text_answer4");

            migrationBuilder.RenameColumn(
                name: "wrong_answer2",
                table: "question_ans",
                newName: "text_answer3");

            migrationBuilder.RenameColumn(
                name: "wrong_answer1",
                table: "question_ans",
                newName: "text_answer2");

            migrationBuilder.AddColumn<string>(
                name: "correct_answer",
                table: "question_ans",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "correct_answer",
                table: "question_ans");

            migrationBuilder.RenameColumn(
                name: "text_answer4",
                table: "question_ans",
                newName: "wrong_answer3");

            migrationBuilder.RenameColumn(
                name: "text_answer3",
                table: "question_ans",
                newName: "wrong_answer2");

            migrationBuilder.RenameColumn(
                name: "text_answer2",
                table: "question_ans",
                newName: "wrong_answer1");
        }
    }
}
