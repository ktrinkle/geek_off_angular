using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace geek_off_angular.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "current_team",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    round_no = table.Column<int>(type: "integer", nullable: false),
                    team_no = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_current_team", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "event_master",
                columns: table => new
                {
                    yevent = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    event_name = table.Column<string>(type: "text", nullable: true),
                    sel_event = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_master", x => x.yevent);
                });

            migrationBuilder.CreateTable(
                name: "fundraising_total",
                columns: table => new
                {
                    yevent = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    totaldollar = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fundraising_total", x => x.yevent);
                });

            migrationBuilder.CreateTable(
                name: "question_ans",
                columns: table => new
                {
                    yevent = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    question_no = table.Column<int>(type: "integer", nullable: false),
                    round_no = table.Column<int>(type: "integer", nullable: false),
                    text_question = table.Column<string>(type: "text", nullable: true),
                    text_answer = table.Column<string>(type: "text", nullable: true),
                    multiple_choice = table.Column<bool>(type: "boolean", nullable: true),
                    wrong_answer1 = table.Column<string>(type: "text", nullable: true),
                    wrong_answer2 = table.Column<string>(type: "text", nullable: true),
                    wrong_answer3 = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_question_ans", x => new { x.yevent, x.round_no, x.question_no });
                });

            migrationBuilder.CreateTable(
                name: "round1score",
                columns: table => new
                {
                    yevent = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    row_name = table.Column<int>(type: "integer", nullable: true),
                    resp_txt = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_round1score", x => x.yevent);
                });

            migrationBuilder.CreateTable(
                name: "roundresult",
                columns: table => new
                {
                    yevent = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    round_no = table.Column<int>(type: "integer", nullable: false),
                    team_no = table.Column<int>(type: "integer", nullable: false),
                    ptswithbonus = table.Column<decimal>(type: "numeric", nullable: true),
                    rnk = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roundresult", x => new { x.yevent, x.round_no, x.team_no });
                });

            migrationBuilder.CreateTable(
                name: "scoreposs",
                columns: table => new
                {
                    yevent = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    round_no = table.Column<int>(type: "integer", nullable: false),
                    question_no = table.Column<int>(type: "integer", nullable: false),
                    survey_order = table.Column<int>(type: "integer", nullable: false),
                    question_answer = table.Column<string>(type: "text", nullable: true),
                    ptsposs = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scoreposs", x => new { x.yevent, x.round_no, x.question_no, x.survey_order });
                });

            migrationBuilder.CreateTable(
                name: "scoring",
                columns: table => new
                {
                    yevent = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    team_no = table.Column<int>(type: "integer", nullable: false),
                    round_no = table.Column<int>(type: "integer", nullable: false),
                    question_no = table.Column<int>(type: "integer", nullable: false),
                    team_answer = table.Column<string>(type: "text", nullable: true),
                    player_num = table.Column<int>(type: "integer", nullable: true),
                    point_amt = table.Column<int>(type: "integer", nullable: true),
                    round3neg = table.Column<int>(type: "integer", nullable: true),
                    final_jep = table.Column<int>(type: "integer", nullable: true),
                    updatetime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scoring", x => new { x.yevent, x.round_no, x.team_no, x.question_no });
                });

            migrationBuilder.CreateTable(
                name: "team_reference",
                columns: table => new
                {
                    yevent = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    team_no = table.Column<int>(type: "integer", nullable: false),
                    teamname = table.Column<string>(type: "text", nullable: true),
                    member1 = table.Column<string>(type: "text", nullable: true),
                    member2 = table.Column<string>(type: "text", nullable: true),
                    dollarraised = table.Column<decimal>(type: "numeric", nullable: true),
                    workgroup1 = table.Column<string>(type: "text", nullable: true),
                    workgroup2 = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_team_reference", x => new { x.yevent, x.team_no });
                });

            migrationBuilder.CreateTable(
                name: "team_user",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    yevent = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: true),
                    team_no = table.Column<int>(type: "integer", nullable: false),
                    badge_id = table.Column<string>(type: "text", nullable: true),
                    username = table.Column<string>(type: "text", nullable: true),
                    admin_flag = table.Column<bool>(type: "boolean", nullable: false),
                    login_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_team_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_answer",
                columns: table => new
                {
                    yevent = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    team_no = table.Column<int>(type: "integer", nullable: false),
                    question_no = table.Column<int>(type: "integer", nullable: false),
                    round_no = table.Column<int>(type: "integer", nullable: false),
                    text_answer = table.Column<string>(type: "text", nullable: true),
                    answer_user = table.Column<string>(type: "text", nullable: true),
                    answer_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_answer", x => new { x.yevent, x.round_no, x.team_no, x.question_no });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "current_team");

            migrationBuilder.DropTable(
                name: "event_master");

            migrationBuilder.DropTable(
                name: "fundraising_total");

            migrationBuilder.DropTable(
                name: "question_ans");

            migrationBuilder.DropTable(
                name: "round1score");

            migrationBuilder.DropTable(
                name: "roundresult");

            migrationBuilder.DropTable(
                name: "scoreposs");

            migrationBuilder.DropTable(
                name: "scoring");

            migrationBuilder.DropTable(
                name: "team_reference");

            migrationBuilder.DropTable(
                name: "team_user");

            migrationBuilder.DropTable(
                name: "user_answer");
        }
    }
}
