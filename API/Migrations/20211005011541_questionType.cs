using Microsoft.EntityFrameworkCore.Migrations;

namespace geek_off_angular.Migrations
{
    public partial class questionType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "match_question",
                table: "question_ans",
                type: "boolean",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "match_question",
                table: "question_ans");
        }
    }
}
