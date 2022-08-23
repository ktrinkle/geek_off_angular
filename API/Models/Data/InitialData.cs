using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GeekOff.Data
{
    public static class ModelBuilderExtensions
    {
        public static readonly List<EventMaster> EventMaster = new()
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

        public static readonly List<QuestionAns> QuestionAns = new()
        {
            new QuestionAns()
            {
                Yevent = "e21",
                QuestionNum = 201,
                RoundNum = 2,
                TextQuestion = "Name your favorite developer."
            }
        };

        public static readonly List<Scoreposs> ScorePoss = new()
        {
            new Scoreposs()
            {
                Yevent = "e21",
                RoundNum = 2,
                QuestionNum = 201,
                SurveyOrder = 1,
                QuestionAnswer = "Kevin",
                Ptsposs = 8
            },
            new Scoreposs()
            {
                Yevent = "e21",
                RoundNum = 2,
                QuestionNum = 201,
                SurveyOrder = 2,
                QuestionAnswer = "Kristin",
                Ptsposs = 7
            },
            new Scoreposs()
            {
                Yevent = "e21",
                RoundNum = 2,
                QuestionNum = 201,
                SurveyOrder = 3,
                QuestionAnswer = "Diyalo",
                Ptsposs = 6
            },
            new Scoreposs()
            {
                Yevent = "e21",
                RoundNum = 2,
                QuestionNum = 201,
                SurveyOrder = 5,
                QuestionAnswer = "Li",
                Ptsposs = 2
            },
            new Scoreposs()
            {
                Yevent = "e21",
                RoundNum = 2,
                QuestionNum = 201,
                SurveyOrder = 4,
                QuestionAnswer = "Dan",
                Ptsposs = 5
            }
        };

        public static readonly List<Teamreference> TeamReference = new()
        {
            new Teamreference()
            {
                Yevent = "e21",
                TeamNum = 1,
                Teamname = "Go Aggies",
                Dollarraised = 1000,
                TeamGuid = new Guid()
            }
        };

        public static readonly List<TeamUser> TeamUser = new()
        {
            new TeamUser()
            {
                Id = 104,
                Yevent = "e21",
                TeamNum = 1,
                BadgeId = "285557",
                Username = "285557@geekoff.onmicrosoft.com",
                PlayerName = "Grant Hill",
                PlayerNum = 1,
                WorkgroupName = "Information Technology",
            },
            new TeamUser()
            {
                Id = 106,
                Yevent = "e21",
                TeamNum = 1,
                Username = "641903@geekoff.onmicrosoft.com",
                PlayerName = "Brandon Heath",
                PlayerNum = 2,
                WorkgroupName = "Information Technology",
            },
            new TeamUser()
            {
                Id = 107,
                Yevent = "e21",
                TeamNum = 2,
                Username = "991023@geekoff.onmicrosoft.com",
                PlayerName = "Roger Marsolek",
                PlayerNum = 1,
                WorkgroupName = "Information Technology",
            },
        };

        public static readonly List<AdminUser> AdminUser = new()
        {
            new AdminUser()
            {
                Id = 100,
                Username = "362525",
                AdminName = "Kevin Trinkle",
            },
            new AdminUser()
            {
                Id = 101,
                Username = "446792",
                AdminName = "Kristin Russell",
            },
            new AdminUser()
            {
                Id = 102,
                Username = "226250",
                AdminName = "Diyalo Manral",
            },
            new AdminUser()
            {
                Id = 103,
                Username = "288132",
                AdminName = "Dan Mullings",
            },
            new AdminUser()
            {
                Id = 105,
                Username = "jay.cox",
                AdminName = "Jay Cox",
            },
        };

        public static void CreateEventMasterData(this ModelBuilder modelBuilder) => modelBuilder.Entity<EventMaster>().HasData(EventMaster.ToArray());

        public static void CreateScorepossData(this ModelBuilder modelBuilder) => modelBuilder.Entity<Scoreposs>().HasData(ScorePoss.ToArray());

        public static void CreateQuestionAnsData(this ModelBuilder modelBuilder) => modelBuilder.Entity<QuestionAns>().HasData(QuestionAns.ToArray());

        public static void CreateTeamReferenceData(this ModelBuilder modelBuilder) => modelBuilder.Entity<Teamreference>().HasData(TeamReference.ToArray());

        public static void CreateTeamUserData(this ModelBuilder modelBuilder) => modelBuilder.Entity<TeamUser>().HasData(TeamUser.ToArray());

        public static void CreateAdminUserData(this ModelBuilder modelBuilder) => modelBuilder.Entity<AdminUser>().HasData(AdminUser.ToArray());
    }


}
