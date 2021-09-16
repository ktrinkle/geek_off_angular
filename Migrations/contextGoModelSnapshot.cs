﻿// <auto-generated />
using System;
using GeekOff.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace geek_off_angular.Migrations
{
    [DbContext(typeof(contextGo))]
    partial class contextGoModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("GeekOff.Data.CurrentTeam", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("RoundNo")
                        .HasColumnType("integer")
                        .HasColumnName("round_no");

                    b.Property<int>("TeamNo")
                        .HasColumnType("integer")
                        .HasColumnName("team_no");

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

            modelBuilder.Entity("GeekOff.Data.QuestionAns", b =>
                {
                    b.Property<string>("Yevent")
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)")
                        .HasColumnName("yevent");

                    b.Property<int>("RoundNo")
                        .HasColumnType("integer")
                        .HasColumnName("round_no");

                    b.Property<int>("QuestionNo")
                        .HasColumnType("integer")
                        .HasColumnName("question_no");

                    b.Property<bool?>("MultipleChoice")
                        .HasColumnType("boolean")
                        .HasColumnName("multiple_choice");

                    b.Property<string>("TextAnswer")
                        .HasColumnType("text")
                        .HasColumnName("text_answer");

                    b.Property<string>("TextQuestion")
                        .HasColumnType("text")
                        .HasColumnName("text_question");

                    b.Property<string>("WrongAnswer1")
                        .HasColumnType("text")
                        .HasColumnName("wrong_answer1");

                    b.Property<string>("WrongAnswer2")
                        .HasColumnType("text")
                        .HasColumnName("wrong_answer2");

                    b.Property<string>("WrongAnswer3")
                        .HasColumnType("text")
                        .HasColumnName("wrong_answer3");

                    b.HasKey("Yevent", "RoundNo", "QuestionNo");

                    b.ToTable("question_ans");

                    b.HasData(
                        new
                        {
                            Yevent = "e21",
                            RoundNo = 2,
                            QuestionNo = 201,
                            TextQuestion = "Name your favorite developer."
                        });
                });

            modelBuilder.Entity("GeekOff.Data.Round1score", b =>
                {
                    b.Property<string>("Yevent")
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)")
                        .HasColumnName("yevent");

                    b.Property<string>("RespTxt")
                        .HasColumnType("text")
                        .HasColumnName("resp_txt");

                    b.Property<int?>("RowName")
                        .HasColumnType("integer")
                        .HasColumnName("row_name");

                    b.HasKey("Yevent");

                    b.ToTable("round1score");
                });

            modelBuilder.Entity("GeekOff.Data.Roundresult", b =>
                {
                    b.Property<string>("Yevent")
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)")
                        .HasColumnName("yevent");

                    b.Property<int>("RoundNo")
                        .HasColumnType("integer")
                        .HasColumnName("round_no");

                    b.Property<int>("TeamNo")
                        .HasColumnType("integer")
                        .HasColumnName("team_no");

                    b.Property<decimal?>("Ptswithbonus")
                        .HasColumnType("numeric")
                        .HasColumnName("ptswithbonus");

                    b.Property<int?>("rnk")
                        .HasColumnType("integer")
                        .HasColumnName("rnk");

                    b.HasKey("Yevent", "RoundNo", "TeamNo");

                    b.ToTable("roundresult");
                });

            modelBuilder.Entity("GeekOff.Data.Scoreposs", b =>
                {
                    b.Property<string>("Yevent")
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)")
                        .HasColumnName("yevent");

                    b.Property<int>("RoundNo")
                        .HasColumnType("integer")
                        .HasColumnName("round_no");

                    b.Property<int>("QuestionNo")
                        .HasColumnType("integer")
                        .HasColumnName("question_no");

                    b.Property<int>("SurveyOrder")
                        .HasColumnType("integer")
                        .HasColumnName("survey_order");

                    b.Property<int?>("Ptsposs")
                        .HasColumnType("integer")
                        .HasColumnName("ptsposs");

                    b.Property<string>("QuestionAnswer")
                        .HasColumnType("text")
                        .HasColumnName("question_answer");

                    b.HasKey("Yevent", "RoundNo", "QuestionNo", "SurveyOrder");

                    b.ToTable("scoreposs");

                    b.HasData(
                        new
                        {
                            Yevent = "e21",
                            RoundNo = 2,
                            QuestionNo = 201,
                            SurveyOrder = 1,
                            Ptsposs = 8,
                            QuestionAnswer = "Kevin"
                        },
                        new
                        {
                            Yevent = "e21",
                            RoundNo = 2,
                            QuestionNo = 201,
                            SurveyOrder = 2,
                            Ptsposs = 7,
                            QuestionAnswer = "Kristin"
                        },
                        new
                        {
                            Yevent = "e21",
                            RoundNo = 2,
                            QuestionNo = 201,
                            SurveyOrder = 3,
                            Ptsposs = 6,
                            QuestionAnswer = "Diyalo"
                        },
                        new
                        {
                            Yevent = "e21",
                            RoundNo = 2,
                            QuestionNo = 201,
                            SurveyOrder = 4,
                            Ptsposs = 6,
                            QuestionAnswer = "Li"
                        },
                        new
                        {
                            Yevent = "e21",
                            RoundNo = 2,
                            QuestionNo = 201,
                            SurveyOrder = 5,
                            Ptsposs = 5,
                            QuestionAnswer = "Dan"
                        });
                });

            modelBuilder.Entity("GeekOff.Data.Scoring", b =>
                {
                    b.Property<string>("Yevent")
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)")
                        .HasColumnName("yevent");

                    b.Property<int>("RoundNo")
                        .HasColumnType("integer")
                        .HasColumnName("round_no");

                    b.Property<int>("TeamNo")
                        .HasColumnType("integer")
                        .HasColumnName("team_no");

                    b.Property<int>("QuestionNo")
                        .HasColumnType("integer")
                        .HasColumnName("question_no");

                    b.Property<int?>("FinalJep")
                        .HasColumnType("integer")
                        .HasColumnName("final_jep");

                    b.Property<int?>("PlayerNum")
                        .HasColumnType("integer")
                        .HasColumnName("player_num");

                    b.Property<int?>("PointAmt")
                        .HasColumnType("integer")
                        .HasColumnName("point_amt");

                    b.Property<int?>("Round3neg")
                        .HasColumnType("integer")
                        .HasColumnName("round3neg");

                    b.Property<string>("TeamAnswer")
                        .HasColumnType("text")
                        .HasColumnName("team_answer");

                    b.Property<DateTime>("Updatetime")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updatetime");

                    b.HasKey("Yevent", "RoundNo", "TeamNo", "QuestionNo");

                    b.ToTable("scoring");
                });

            modelBuilder.Entity("GeekOff.Data.TeamUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("AdminFlag")
                        .HasColumnType("boolean")
                        .HasColumnName("admin_flag");

                    b.Property<string>("BadgeId")
                        .HasColumnType("text")
                        .HasColumnName("badge_id");

                    b.Property<DateTime>("LoginTime")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("login_time");

                    b.Property<int>("TeamNo")
                        .HasColumnType("integer")
                        .HasColumnName("team_no");

                    b.Property<string>("Username")
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.Property<string>("Yevent")
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)")
                        .HasColumnName("yevent");

                    b.HasKey("Id");

                    b.ToTable("team_user");
                });

            modelBuilder.Entity("GeekOff.Data.Teamreference", b =>
                {
                    b.Property<string>("Yevent")
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)")
                        .HasColumnName("yevent");

                    b.Property<int>("TeamNo")
                        .HasColumnType("integer")
                        .HasColumnName("team_no");

                    b.Property<decimal?>("Dollarraised")
                        .HasColumnType("numeric")
                        .HasColumnName("dollarraised");

                    b.Property<string>("Member1")
                        .HasColumnType("text")
                        .HasColumnName("member1");

                    b.Property<string>("Member2")
                        .HasColumnType("text")
                        .HasColumnName("member2");

                    b.Property<string>("Teamname")
                        .HasColumnType("text")
                        .HasColumnName("teamname");

                    b.Property<string>("Workgroup1")
                        .HasColumnType("text")
                        .HasColumnName("workgroup1");

                    b.Property<string>("Workgroup2")
                        .HasColumnType("text")
                        .HasColumnName("workgroup2");

                    b.HasKey("Yevent", "TeamNo");

                    b.ToTable("team_reference");
                });

            modelBuilder.Entity("GeekOff.Data.UserAnswer", b =>
                {
                    b.Property<string>("Yevent")
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)")
                        .HasColumnName("yevent");

                    b.Property<int?>("RoundNo")
                        .HasColumnType("integer")
                        .HasColumnName("round_no");

                    b.Property<int>("TeamNo")
                        .HasColumnType("integer")
                        .HasColumnName("team_no");

                    b.Property<int>("QuestionNo")
                        .HasColumnType("integer")
                        .HasColumnName("question_no");

                    b.Property<DateTime>("AnswerTime")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("answer_time");

                    b.Property<string>("AnswerUser")
                        .HasColumnType("text")
                        .HasColumnName("answer_user");

                    b.Property<string>("TextAnswer")
                        .HasColumnType("text")
                        .HasColumnName("text_answer");

                    b.HasKey("Yevent", "RoundNo", "TeamNo", "QuestionNo");

                    b.ToTable("user_answer");
                });
#pragma warning restore 612, 618
        }
    }
}
