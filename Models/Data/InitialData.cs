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
            },
            new Scoreposs()
            {
                Yevent = "e21",
                RoundNo = 2,
                QuestionNo = 201,
                SurveyOrder = 6,
                QuestionAnswer = "Grant",
                Ptsposs = 5
            }
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
    }


}