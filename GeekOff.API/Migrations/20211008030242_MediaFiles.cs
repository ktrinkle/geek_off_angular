using Microsoft.EntityFrameworkCore.Migrations;

namespace geek_off_angular.Migrations
{
    public partial class MediaFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "media_file",
                table: "question_ans",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "media_type",
                table: "question_ans",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "media_file",
                table: "question_ans");

            migrationBuilder.DropColumn(
                name: "media_type",
                table: "question_ans");
        }
    }
}
