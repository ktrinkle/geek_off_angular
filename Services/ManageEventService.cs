using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using GeekOff.Data;
using GeekOff.Models;

namespace GeekOff.Services
{
    public class ManageEventService : IManageEventService
    {
        private readonly contextGo _contextGo;
        private readonly ILogger<ManageEventService> _logger;
        public ManageEventService(ILogger<ManageEventService> logger, contextGo context)
        {
            _logger = logger;
            _contextGo = context;
        }

        public async Task<List<Round2SurveyList>> GetRound2SurveyMaster(string yEvent)
        {
            var surveyAnswer = await _contextGo.Scoreposs.Where(q => q.Yevent == yEvent && q.RoundNo == 2)
                                        .ToListAsync();

            // get the question text
            var surveyReturn = await GetRound2QuestionList(yEvent);

            foreach (Round2SurveyList survey in surveyReturn)
            {
                survey.SurveyAnswers = surveyAnswer.FindAll(s => s.QuestionNo == survey.QuestionNo)
                                                    .Select(s => new Round2Answers()
                                                    {
                                                        QuestionNo = s.QuestionNo,
                                                        Answer = s.QuestionAnswer,
                                                        Score = (int)s.Ptsposs
                                                    }).ToList();
            }

            return surveyReturn;            
        }

        public async Task<List<Round2SurveyList>> GetRound2QuestionList(string yEvent)
        {
            // get the question text
            var surveyReturn = await _contextGo.QuestionAns.Where(q => q.Yevent == yEvent && q.RoundNo == 2)
                                        .Select(q => new Round2SurveyList()
                                            {
                                                QuestionNo = q.QuestionNo,
                                                QuestionText = q.TextQuestion
                                            })
                                        .ToListAsync();

            return surveyReturn;            
        }

        public async Task<string> SetRound2AnswerText(string yEvent, int questionNum, int teamNum, int playerNum, string answer, int score)
        {
            // check limits
            if (!(playerNum > 1 && playerNum < 3))
            {
                var err = "The player number is not valid. Please try again.";
                return err;
            }

            if (questionNum < 200 || questionNum > 299)
            {
                return "The question is not a valid round 2 question. Please try again.";
            }

            if (teamNum < 1)
            {
                return "A valid team number is required.";
            }

            var scoreRecord = new Scoring()
            {
                Yevent = yEvent,
                TeamNo = teamNum,
                RoundNo = 2,
                QuestionNo = questionNum,
                TeamAnswer = answer,
                PlayerNum = playerNum,
                PointAmt = score,
                Updatetime = DateTime.UtcNow
            };

            await _contextGo.AddAsync(scoreRecord);
            await _contextGo.SaveChangesAsync();

            return "The answer was successfully saved.";
        }

        public async Task<string> SetRound2AnswerSurvey(string yEvent, int questionNum, int teamNum, int playerNum, int answerNum)
        {
            // check limits
            if (!(playerNum > 1 && playerNum < 3))
            {
                var err = "The player number is not valid. Please try again.";
                return err;
            }

            if (questionNum < 200 || questionNum > 299)
            {
                return "The question is not a valid round 2 question. Please try again.";
            }

            if (teamNum < 1)
            {
                return "A valid team number is required.";
            }

            if (answerNum < 0 || answerNum > 5)
            {
                return "A valid answer number is required.";
            }

            var answerText = await _contextGo.Scoreposs.SingleOrDefaultAsync(q => q.Yevent == yEvent && q.RoundNo == 2 && q.QuestionNo == questionNum && q.SurveyOrder == answerNum);
            if (answerText == null)
            {
                return "Invalid answer number.";
            }

            var scoreRecord = new Scoring()
            {
                Yevent = yEvent,
                TeamNo = teamNum,
                RoundNo = 2,
                QuestionNo = questionNum,
                TeamAnswer = answerText.QuestionAnswer.Substring(0,11),
                PlayerNum = playerNum,
                PointAmt = answerText.Ptsposs,
                Updatetime = DateTime.UtcNow
            };

            await _contextGo.AddAsync(scoreRecord);
            await _contextGo.SaveChangesAsync();

            return "The answer was successfully saved.";
        }

        public async Task<string> FinalizeRound(string yEvent)
        {
            if (yEvent is null)
            {
                return "No event was specified.";
            }

            var totalPoints = await _contextGo.Scoring.Where(s => s.RoundNo == 2 && s.Yevent == yEvent)
                                .GroupBy(s => s.TeamNo)
                                .Select(s => new Round2FinalDto()
                                {
                                    TeamNo = s.Key,
                                    FinalScore = s.Sum(s => s.PointAmt)
                                }).ToListAsync();

            // now we rank and store into the DB. First we remove anything that already exists.

            var scorestoRemove = await _contextGo.Roundresult.Where(r => r.Yevent == yEvent && r.RoundNo == 2).ToListAsync();

            if (!(scorestoRemove is null))
            {
                _contextGo.RemoveRange(scorestoRemove);
                await _contextGo.SaveChangesAsync();
            }

            // add the new records
            var scorestoAdd = from s in totalPoints
            orderby s.FinalScore descending
            select new Roundresult()
            {
                Yevent = yEvent,
                TeamNo = s.TeamNo,
                RoundNo = 2,
                Ptswithbonus = s.FinalScore,
                rnk = (from r in totalPoints
                        where r.FinalScore > s.FinalScore
                        select r).Count() + 1
            };

            return "Scores have been finalized in the system.";

        }
    }
}
