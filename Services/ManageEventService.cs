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

        public async Task<string> SetRound2AnswerText(Round2AnswerDto submitAnswer)
        {
            // check limits
            if (!(submitAnswer.PlayerNum > 0 && submitAnswer.PlayerNum < 3))
            {
                var err = "The player number is not valid. Please try again.";
                return err;
            }

            if (submitAnswer.QuestionNum < 200 || submitAnswer.QuestionNum > 299)
            {
                return "The question is not a valid round 2 question. Please try again.";
            }

            if (submitAnswer.TeamNum < 1)
            {
                return "A valid team number is required.";
            }

            var scoreRecord = new Scoring()
            {
                Yevent = submitAnswer.YEvent,
                TeamNo = submitAnswer.TeamNum,
                RoundNo = 2,
                QuestionNo = submitAnswer.QuestionNum,
                TeamAnswer = submitAnswer.Answer,
                PlayerNum = submitAnswer.PlayerNum,
                PointAmt = submitAnswer.Score,
                Updatetime = DateTime.UtcNow
            };

            await _contextGo.AddAsync(scoreRecord);
            await _contextGo.SaveChangesAsync();

            return "The answer was successfully saved.";
        }

        public async Task<string> SetRound2AnswerSurvey(Round2AnswerDto submitAnswer)
        {
            // check limits
            if (!(submitAnswer.PlayerNum > 0 && submitAnswer.PlayerNum < 3))
            {
                var err = "The player number is not valid. Please try again.";
                return err;
            }

            if (submitAnswer.QuestionNum < 200 || submitAnswer.QuestionNum > 299)
            {
                return "The question is not a valid round 2 question. Please try again.";
            }

            if (submitAnswer.TeamNum < 1)
            {
                return "A valid team number is required.";
            }

            if (submitAnswer.AnswerNum < 0 || submitAnswer.AnswerNum > 99)
            {
                return "A valid answer number is required.";
            }

            var answerText = await _contextGo.Scoreposs.SingleOrDefaultAsync(q => q.Yevent == submitAnswer.YEvent && q.RoundNo == 2 && q.QuestionNo == submitAnswer.QuestionNum && q.SurveyOrder == submitAnswer.AnswerNum);
            if (answerText == null)
            {
                return "Invalid answer number.";
            }

            var scoreRecord = new Scoring()
            {
                Yevent = submitAnswer.YEvent,
                TeamNo = submitAnswer.TeamNum,
                RoundNo = 2,
                QuestionNo = submitAnswer.QuestionNum,
                TeamAnswer = answerText.QuestionAnswer.Substring(0,11),
                PlayerNum = submitAnswer.PlayerNum,
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
