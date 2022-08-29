﻿// <auto-generated />
using System;
using GeekOff.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace geek_off_angular.Migrations
{
    [DbContext(typeof(ContextGo))]
    [Migration("20220825005253_AdminChanges")]
    partial class AdminChanges
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GeekOff.Data.AdminUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("AdminName")
                        .HasColumnType("text")
                        .HasColumnName("admin_name");

                    b.Property<DateTime>("LoginTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("login_time");

                    b.Property<string>("Username")
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.ToTable("admin_user");

                    b.HasData(
                        new
                        {
                            Id = 100L,
                            AdminName = "Kevin Trinkle",
                            LoginTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Username = "362525"
                        },
                        new
                        {
                            Id = 101L,
                            AdminName = "Kristin Russell",
                            LoginTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Username = "446792"
                        },
                        new
                        {
                            Id = 102L,
                            AdminName = "Diyalo Manral",
                            LoginTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Username = "226250"
                        },
                        new
                        {
                            Id = 103L,
                            AdminName = "Dan Mullings",
                            LoginTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Username = "288132"
                        },
                        new
                        {
                            Id = 105L,
                            AdminName = "Jay Cox",
                            LoginTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Username = "jaycox"
                        });
                });

            modelBuilder.Entity("GeekOff.Data.CurrentQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("QuestionNum")
                        .HasColumnType("integer")
                        .HasColumnName("question_num");

                    b.Property<DateTime>("QuestionTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("question_time");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<string>("YEvent")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("y_event");

                    b.HasKey("Id");

                    b.ToTable("current_question");
                });

            modelBuilder.Entity("GeekOff.Data.CurrentTeam", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("RoundNum")
                        .HasColumnType("integer")
                        .HasColumnName("round_num");

                    b.Property<int>("TeamNum")
                        .HasColumnType("integer")
                        .HasColumnName("team_num");

                    b.HasKey("Id");

                    b.ToTable("current_team");
                });

            modelBuilder.Entity("GeekOff.Data.EventMaster", b =>
                {
                    b.Property<string>("Yevent")
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)")
                        .HasColumnName("yevent");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("event_name");

                    b.Property<bool?>("SelEvent")
                        .HasColumnType("boolean")
                        .HasColumnName("sel_event");

                    b.HasKey("Yevent");

                    b.ToTable("event_master");

                    b.HasData(
                        new
                        {
                            Yevent = "e21",
                            EventName = "Employee 2021",
                            SelEvent = true
                        },
                        new
                        {
                            Yevent = "e19",
                            EventName = "Employee 2019"
                        });
                });

            modelBuilder.Entity("GeekOff.Data.FundraisingTotal", b =>
                {
                    b.Property<string>("Yevent")
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)")
                        .HasColumnName("yevent");

                    b.Property<decimal?>("Totaldollar")
                        .HasColumnType("numeric")
                        .HasColumnName("totaldollar");

                    b.HasKey("Yevent");

                    b.ToTable("fundraising_total");
                });

            modelBuilder.Entity("GeekOff.Data.LogError", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ErrorMessage")
                        .HasColumnType("text")
                        .HasColumnName("error_message");

                    b.HasKey("Id");

                    b.ToTable("log_error");
                });

            modelBuilder.Entity("GeekOff.Data.QuestionAns", b =>
                {
                    b.Property<string>("Yevent")
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)")
                        .HasColumnName("yevent");

                    b.Property<int>("RoundNum")
                        .HasColumnType("integer")
                        .HasColumnName("round_num");

                    b.Property<int>("QuestionNum")
                        .HasColumnType("integer")
                        .HasColumnName("question_num");

                    b.Property<string>("CorrectAnswer")
                        .HasColumnType("text")
                        .HasColumnName("correct_answer");

                    b.Property<bool>("DailyDouble")
                        .HasColumnType("boolean")
                        .HasColumnName("daily_double");

                    b.Property<bool?>("MatchQuestion")
                        .HasColumnType("boolean")
                        .HasColumnName("match_question");

                    b.Property<string>("MediaFile")
                        .HasColumnType("text")
                        .HasColumnName("media_file");

                    b.Property<string>("MediaType")
                        .HasColumnType("text")
                        .HasColumnName("media_type");

                    b.Property<bool?>("MultipleChoice")
                        .HasColumnType("boolean")
                        .HasColumnName("multiple_choice");

                    b.Property<string>("TextAnswer")
                        .HasColumnType("text")
                        .HasColumnName("text_answer");

                    b.Property<string>("TextAnswer2")
                        .HasColumnType("text")
                        .HasColumnName("text_answer2");

                    b.Property<string>("TextAnswer3")
                        .HasColumnType("text")
                        .HasColumnName("text_answer3");

                    b.Property<string>("TextAnswer4")
                        .HasColumnType("text")
                        .HasColumnName("text_answer4");

                    b.Property<string>("TextQuestion")
                        .HasColumnType("text")
                        .HasColumnName("text_question");

                    b.HasKey("Yevent", "RoundNum", "QuestionNum");

                    b.ToTable("question_ans");

                    b.HasData(
                        new
                        {
                            Yevent = "e21",
                            RoundNum = 2,
                            QuestionNum = 201,
                            DailyDouble = false,
                            TextQuestion = "Name your favorite developer."
                        });
                });

            modelBuilder.Entity("GeekOff.Data.Roundresult", b =>
                {
                    b.Property<string>("Yevent")
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)")
                        .HasColumnName("yevent");

                    b.Property<int>("RoundNum")
                        .HasColumnType("integer")
                        .HasColumnName("round_num");

                    b.Property<int>("TeamNum")
                        .HasColumnType("integer")
                        .HasColumnName("team_num");

                    b.Property<decimal?>("Ptswithbonus")
                        .HasColumnType("numeric")
                        .HasColumnName("ptswithbonus");

                    b.Property<int?>("Rnk")
                        .HasColumnType("integer")
                        .HasColumnName("rnk");

                    b.HasKey("Yevent", "RoundNum", "TeamNum");

                    b.ToTable("roundresult");
                });

            modelBuilder.Entity("GeekOff.Data.Scoreposs", b =>
                {
                    b.Property<string>("Yevent")
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)")
                        .HasColumnName("yevent");

                    b.Property<int>("RoundNum")
                        .HasColumnType("integer")
                        .HasColumnName("round_num");

                    b.Property<int>("QuestionNum")
                        .HasColumnType("integer")
                        .HasColumnName("question_num");

                    b.Property<int>("SurveyOrder")
                        .HasColumnType("integer")
                        .HasColumnName("survey_order");

                    b.Property<int?>("Ptsposs")
                        .HasColumnType("integer")
                        .HasColumnName("ptsposs");

                    b.Property<string>("QuestionAnswer")
                        .HasColumnType("text")
                        .HasColumnName("question_answer");

                    b.HasKey("Yevent", "RoundNum", "QuestionNum", "SurveyOrder");

                    b.ToTable("scoreposs");

                    b.HasData(
                        new
                        {
                            Yevent = "e21",
                            RoundNum = 2,
                            QuestionNum = 201,
                            SurveyOrder = 1,
                            Ptsposs = 8,
                            QuestionAnswer = "Kevin"
                        },
                        new
                        {
                            Yevent = "e21",
                            RoundNum = 2,
                            QuestionNum = 201,
                            SurveyOrder = 2,
                            Ptsposs = 7,
                            QuestionAnswer = "Kristin"
                        },
                        new
                        {
                            Yevent = "e21",
                            RoundNum = 2,
                            QuestionNum = 201,
                            SurveyOrder = 3,
                            Ptsposs = 6,
                            QuestionAnswer = "Diyalo"
                        },
                        new
                        {
                            Yevent = "e21",
                            RoundNum = 2,
                            QuestionNum = 201,
                            SurveyOrder = 5,
                            Ptsposs = 2,
                            QuestionAnswer = "Li"
                        },
                        new
                        {
                            Yevent = "e21",
                            RoundNum = 2,
                            QuestionNum = 201,
                            SurveyOrder = 4,
                            Ptsposs = 5,
                            QuestionAnswer = "Dan"
                        });
                });

            modelBuilder.Entity("GeekOff.Data.Scoring", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("FinalJep")
                        .HasColumnType("integer")
                        .HasColumnName("final_jep");

                    b.Property<int?>("PlayerNum")
                        .HasColumnType("integer")
                        .HasColumnName("player_num");

                    b.Property<int?>("PointAmt")
                        .HasColumnType("integer")
                        .HasColumnName("point_amt");

                    b.Property<int>("QuestionNum")
                        .HasColumnType("integer")
                        .HasColumnName("question_num");

                    b.Property<int?>("Round3neg")
                        .HasColumnType("integer")
                        .HasColumnName("round3neg");

                    b.Property<int>("RoundNum")
                        .HasColumnType("integer")
                        .HasColumnName("round_num");

                    b.Property<string>("TeamAnswer")
                        .HasColumnType("text")
                        .HasColumnName("team_answer");

                    b.Property<int>("TeamNum")
                        .HasColumnType("integer")
                        .HasColumnName("team_num");

                    b.Property<DateTime>("Updatetime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updatetime");

                    b.Property<string>("Yevent")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)")
                        .HasColumnName("yevent");

                    b.HasKey("Id");

                    b.ToTable("scoring");
                });

            modelBuilder.Entity("GeekOff.Data.Teamreference", b =>
                {
                    b.Property<string>("Yevent")
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)")
                        .HasColumnName("yevent");

                    b.Property<int>("TeamNum")
                        .HasColumnType("integer")
                        .HasColumnName("team_num");

                    b.Property<decimal?>("Dollarraised")
                        .HasColumnType("numeric")
                        .HasColumnName("dollarraised");

                    b.Property<DateTime>("LoginTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("login_time");

                    b.Property<Guid>("TeamGuid")
                        .HasColumnType("uuid")
                        .HasColumnName("team_guid");

                    b.Property<string>("Teamname")
                        .HasColumnType("text")
                        .HasColumnName("teamname");

                    b.HasKey("Yevent", "TeamNum");

                    b.ToTable("team_reference");

                    b.HasData(
                        new
                        {
                            Yevent = "e21",
                            TeamNum = 1,
                            Dollarraised = 1000m,
                            LoginTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TeamGuid = new Guid("00000000-0000-0000-0000-000000000000"),
                            Teamname = "Go Aggies"
                        });
                });

            modelBuilder.Entity("GeekOff.Data.TeamUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("BadgeId")
                        .HasColumnType("text")
                        .HasColumnName("badge_id");

                    b.Property<DateTime>("LoginTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("login_time");

                    b.Property<string>("PlayerName")
                        .HasColumnType("text")
                        .HasColumnName("player_name");

                    b.Property<int?>("PlayerNum")
                        .HasColumnType("integer")
                        .HasColumnName("player_num");

                    b.Property<Guid>("SessionId")
                        .HasColumnType("uuid")
                        .HasColumnName("session_id");

                    b.Property<int>("TeamNum")
                        .HasColumnType("integer")
                        .HasColumnName("team_num");

                    b.Property<string>("Username")
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.Property<string>("WorkgroupName")
                        .HasColumnType("text")
                        .HasColumnName("workgroup_name");

                    b.Property<string>("Yevent")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)")
                        .HasColumnName("yevent");

                    b.HasKey("Id");

                    b.ToTable("team_user");

                    b.HasData(
                        new
                        {
                            Id = 104L,
                            BadgeId = "285557",
                            LoginTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PlayerName = "Grant Hill",
                            PlayerNum = 1,
                            SessionId = new Guid("00000000-0000-0000-0000-000000000000"),
                            TeamNum = 1,
                            Username = "285557@geekoff.onmicrosoft.com",
                            WorkgroupName = "Information Technology",
                            Yevent = "e21"
                        },
                        new
                        {
                            Id = 106L,
                            LoginTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PlayerName = "Brandon Heath",
                            PlayerNum = 2,
                            SessionId = new Guid("00000000-0000-0000-0000-000000000000"),
                            TeamNum = 1,
                            Username = "641903@geekoff.onmicrosoft.com",
                            WorkgroupName = "Information Technology",
                            Yevent = "e21"
                        },
                        new
                        {
                            Id = 107L,
                            LoginTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PlayerName = "Roger Marsolek",
                            PlayerNum = 1,
                            SessionId = new Guid("00000000-0000-0000-0000-000000000000"),
                            TeamNum = 2,
                            Username = "991023@geekoff.onmicrosoft.com",
                            WorkgroupName = "Information Technology",
                            Yevent = "e21"
                        });
                });

            modelBuilder.Entity("GeekOff.Data.UserAnswer", b =>
                {
                    b.Property<string>("Yevent")
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)")
                        .HasColumnName("yevent");

                    b.Property<int?>("RoundNum")
                        .HasColumnType("integer")
                        .HasColumnName("round_num");

                    b.Property<int>("TeamNum")
                        .HasColumnType("integer")
                        .HasColumnName("team_num");

                    b.Property<int>("QuestionNum")
                        .HasColumnType("integer")
                        .HasColumnName("question_num");

                    b.Property<DateTime>("AnswerTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("answer_time");

                    b.Property<string>("AnswerUser")
                        .HasColumnType("text")
                        .HasColumnName("answer_user");

                    b.Property<string>("TextAnswer")
                        .HasColumnType("text")
                        .HasColumnName("text_answer");

                    b.HasKey("Yevent", "RoundNum", "TeamNum", "QuestionNum");

                    b.ToTable("user_answer");
                });
#pragma warning restore 612, 618
        }
    }
}
