using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GeekOff.Data
{
    public static class ModelBuilderExtensions
    {
        public static readonly List<EventMaster> eventMaster = new List<EventMaster>() 
        {
            new EventMaster()
            {
                Yevent = "e21",
                EventName = "Employee 2021",
                SelEvent = true
            },
            new EventMaster()
            {
                Yevent = "e19",
                EventName = "Employee 2019"
            }
        };

        public static readonly List<QuestionAns> questionAns = new List<QuestionAns>()
        {
            new QuestionAns()
            {
                Yevent = "e21",
                QuestionNo = 201,
                RoundNo = 2,
                TextQuestion = "Name your favorite developer."
            }
        };

        public static readonly List<Scoreposs> scorePoss = new List<Scoreposs>() 
        {
            new Scoreposs()
            {
                Yevent = "e21",
                RoundNo = 2,
                QuestionNo = 201,
                SurveyOrder = 1,
                QuestionAnswer = "Kevin",
                Ptsposs = 8
            },
            new Scoreposs()
            {
                Yevent = "e21",
                RoundNo = 2,
                QuestionNo = 201,
                SurveyOrder = 2,
                QuestionAnswer = "Kristin",
                Ptsposs = 7
            },
            new Scoreposs()
            {
                Yevent = "e21",
                RoundNo = 2,
                QuestionNo = 201,
                SurveyOrder = 3,
                QuestionAnswer = "Diyalo",
                Ptsposs = 6
            },
            new Scoreposs()
            {
                Yevent = "e21",
                RoundNo = 2,
                QuestionNo = 201,
                SurveyOrder = 4,
                QuestionAnswer = "Li",
                Ptsposs = 6
            },
            new Scoreposs()
            {
                Yevent = "e21",
                RoundNo = 2,
                QuestionNo = 201,
                SurveyOrder = 5,
                QuestionAnswer = "Dan",
                Ptsposs = 5
            }
        };

        public static readonly List<Teamreference> teamReference = new List<Teamreference>() 
        {
            new Teamreference()
            {
                Yevent = "e21",
                TeamNo = 1,
                Teamname = "Go Aggies",
                Member1 = "Grant Hill",
                Member2 = "Brandon Heath",
                Dollarraised = 1000,
                Workgroup1 = "IT",
                Workgroup2 = "IT"
            }
        };

        public static readonly List<TeamUser> teamUser = new List<TeamUser>()
        {
            new TeamUser()
            {
                Id = 100,
                Yevent = "e21",
                TeamNo = 0,
                BadgeId = "362525",
                Username = "362525@geekoff.onmicrosoft.com",
                AdminFlag = true
            },
            new TeamUser()
            {
                Id = 101,
                Yevent = "e21",
                TeamNo = 0,
                BadgeId = "446792",
                Username = "446792@geekoff.onmicrosoft.com",
                AdminFlag = true
            },
            new TeamUser()
            {
                Id = 102,
                Yevent = "e21",
                TeamNo = 0,
                BadgeId = "226250",
                Username = "226250@geekoff.onmicrosoft.com",
                AdminFlag = true
            },
            new TeamUser()
            {
                Id = 103,
                Yevent = "e21",
                TeamNo = 0,
                BadgeId = "288132",
                Username = "288132@geekoff.onmicrosoft.com",
                AdminFlag = true
            },
            new TeamUser()
            {
                Id = 104,
                Yevent = "e21",
                TeamNo = 1,
                BadgeId = "285557",
                Username = "285557@geekoff.onmicrosoft.com",
                PlayerName = "Grant Hill",
                PlayerNum = 1,
                WorkgroupName = "Information Technology",
                AdminFlag = true
            },
            new TeamUser()
            {
                Id = 105,
                Yevent = "e21",
                TeamNo = 0,
                BadgeId = "274798",
                Username = "274798@geekoff.onmicrosoft.com",
                AdminFlag = true
            },   
            new TeamUser()
            {
                Id = 106,
                Yevent = "e21",
                TeamNo = 1,
                BadgeId = "641903",
                Username = "641903@geekoff.onmicrosoft.com",
                PlayerName = "Brandon Heath",
                PlayerNum = 2,
                WorkgroupName = "Information Technology",
                AdminFlag = false
            },         
        };

        public static void CreateEventMasterData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventMaster>().HasData(eventMaster.ToArray());
        }

        public static void CreateScorepossData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Scoreposs>().HasData(scorePoss.ToArray());
        }

        public static void CreateQuestionAnsData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuestionAns>().HasData(questionAns.ToArray());
        }

        public static void CreateTeamReferenceData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Teamreference>().HasData(teamReference.ToArray());
        }

        public static void CreateTeamUserData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeamUser>().HasData(teamUser.ToArray());
        }
    }


}