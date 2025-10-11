using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace geek_off_angular.Migrations
{
    public partial class RenameCols_2022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "member1",
                table: "team_reference");

            migrationBuilder.DropColumn(
                name: "member2",
                table: "team_reference");

            migrationBuilder.DropColumn(
                name: "workgroup1",
                table: "team_reference");

            migrationBuilder.DropColumn(
                name: "workgroup2",
                table: "team_reference");

            migrationBuilder.RenameColumn(
                name: "question_no",
                table: "user_answer",
                newName: "question_num");

            migrationBuilder.RenameColumn(
                name: "team_no",
                table: "user_answer",
                newName: "team_num");

            migrationBuilder.RenameColumn(
                name: "round_no",
                table: "user_answer",
                newName: "round_num");

            migrationBuilder.RenameColumn(
                name: "team_no",
                table: "team_user",
                newName: "team_num");

            migrationBuilder.RenameColumn(
                name: "team_no",
                table: "team_reference",
                newName: "team_num");

            migrationBuilder.RenameColumn(
                name: "team_no",
                table: "scoring",
                newName: "team_num");

            migrationBuilder.RenameColumn(
                name: "round_no",
                table: "scoring",
                newName: "round_num");

            migrationBuilder.RenameColumn(
                name: "question_no",
                table: "scoring",
                newName: "question_num");

            migrationBuilder.RenameColumn(
                name: "question_no",
                table: "scoreposs",
                newName: "question_num");

            migrationBuilder.RenameColumn(
                name: "round_no",
                table: "scoreposs",
                newName: "round_num");

            migrationBuilder.RenameColumn(
                name: "team_no",
                table: "roundresult",
                newName: "team_num");

            migrationBuilder.RenameColumn(
                name: "round_no",
                table: "roundresult",
                newName: "round_num");

            migrationBuilder.RenameColumn(
                name: "question_no",
                table: "question_ans",
                newName: "question_num");

            migrationBuilder.RenameColumn(
                name: "round_no",
                table: "question_ans",
                newName: "round_num");

            migrationBuilder.RenameColumn(
                name: "team_no",
                table: "current_team",
                newName: "team_num");

            migrationBuilder.RenameColumn(
                name: "round_no",
                table: "current_team",
                newName: "round_num");

            migrationBuilder.AlterColumn<DateTime>(
                name: "answer_time",
                table: "user_answer",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<string>(
                name: "yevent",
                table: "team_user",
                type: "character varying(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(6)",
                oldMaxLength: 6,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "login_time",
                table: "team_user",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddColumn<Guid>(
                name: "session_id",
                table: "team_user",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "team_guid",
                table: "team_reference",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "yevent",
                table: "scoring",
                type: "character varying(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(6)",
                oldMaxLength: 6,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updatetime",
                table: "scoring",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddColumn<bool>(
                name: "daily_double",
                table: "question_ans",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "event_name",
                table: "event_master",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "y_event",
                table: "current_question",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "question_time",
                table: "current_question",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "session_id",
                table: "team_user");

            migrationBuilder.DropColumn(
                name: "team_guid",
                table: "team_reference");

            migrationBuilder.DropColumn(
                name: "daily_double",
                table: "question_ans");

            migrationBuilder.RenameColumn(
                name: "question_num",
                table: "user_answer",
                newName: "question_no");

            migrationBuilder.RenameColumn(
                name: "team_num",
                table: "user_answer",
                newName: "team_no");

            migrationBuilder.RenameColumn(
                name: "round_num",
                table: "user_answer",
                newName: "round_no");

            migrationBuilder.RenameColumn(
                name: "team_num",
                table: "team_user",
                newName: "team_no");

            migrationBuilder.RenameColumn(
                name: "team_num",
                table: "team_reference",
                newName: "team_no");

            migrationBuilder.RenameColumn(
                name: "team_num",
                table: "scoring",
                newName: "team_no");

            migrationBuilder.RenameColumn(
                name: "round_num",
                table: "scoring",
                newName: "round_no");

            migrationBuilder.RenameColumn(
                name: "question_num",
                table: "scoring",
                newName: "question_no");

            migrationBuilder.RenameColumn(
                name: "question_num",
                table: "scoreposs",
                newName: "question_no");

            migrationBuilder.RenameColumn(
                name: "round_num",
                table: "scoreposs",
                newName: "round_no");

            migrationBuilder.RenameColumn(
                name: "team_num",
                table: "roundresult",
                newName: "team_no");

            migrationBuilder.RenameColumn(
                name: "round_num",
                table: "roundresult",
                newName: "round_no");

            migrationBuilder.RenameColumn(
                name: "question_num",
                table: "question_ans",
                newName: "question_no");

            migrationBuilder.RenameColumn(
                name: "round_num",
                table: "question_ans",
                newName: "round_no");

            migrationBuilder.RenameColumn(
                name: "team_num",
                table: "current_team",
                newName: "team_no");

            migrationBuilder.RenameColumn(
                name: "round_num",
                table: "current_team",
                newName: "round_no");

            migrationBuilder.AlterColumn<DateTime>(
                name: "answer_time",
                table: "user_answer",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "yevent",
                table: "team_user",
                type: "character varying(6)",
                maxLength: 6,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(6)",
                oldMaxLength: 6);

            migrationBuilder.AlterColumn<DateTime>(
                name: "login_time",
                table: "team_user",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "member1",
                table: "team_reference",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "member2",
                table: "team_reference",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "workgroup1",
                table: "team_reference",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "workgroup2",
                table: "team_reference",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "yevent",
                table: "scoring",
                type: "character varying(6)",
                maxLength: 6,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(6)",
                oldMaxLength: 6);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updatetime",
                table: "scoring",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "event_name",
                table: "event_master",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "y_event",
                table: "current_question",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "question_time",
                table: "current_question",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.UpdateData(
                table: "team_reference",
                keyColumns: new[] { "team_no", "yevent" },
                keyValues: new object[] { 1, "e21" },
                columns: new[] { "member1", "member2", "workgroup1", "workgroup2" },
                values: new object[] { "Grant Hill", "Brandon Heath", "IT", "IT" });
        }
    }
}
