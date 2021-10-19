using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekOff.Data;
using GeekOff.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GeekOff.Services
{
    public class ManageEventService : IManageEventService
    {
        private readonly ContextGo _contextGo;
        private readonly ILogger<ManageEventService> _logger;
        public ManageEventService(ILogger<ManageEventService> logger, ContextGo context)
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

            foreach (var survey in surveyReturn)
            {
                survey.SurveyAnswers = surveyAnswer.FindAll(s => s.QuestionNo == survey.QuestionNum)
                                                    .Select(s => new Round2Answers()
                                                    {
                                                        QuestionNum = s.QuestionNo,
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
                                            QuestionNum = q.QuestionNo,
                                            QuestionText = q.TextQuestion
                                        })
                                        .ToListAsync();

            return surveyReturn;
        }

        public async Task<string> SetRound2AnswerText(Round2AnswerDto submitAnswer)
        {
            // check limits
            if (submitAnswer.PlayerNum is not (> 0 and < 3))
            {
                var err = "The player number is not valid. Please try again.";
                return err;
            }

            if (submitAnswer.QuestionNum is < 200 or > 299)
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

            // check if score record exists. If so, nuke it.
            var recordExists = await _contextGo.Scoring.AnyAsync(s => s.Yevent == submitAnswer.YEvent
                                                        && s.TeamNo == submitAnswer.TeamNum
                                                        && s.QuestionNo == submitAnswer.QuestionNum);

            if (recordExists)
            {
                _contextGo.Scoring.UpdateRange(scoreRecord);
                await _contextGo.SaveChangesAsync();
            }

            if (!recordExists)
            {
                await _contextGo.AddAsync(scoreRecord);
                await _contextGo.SaveChangesAsync();
            }

            return "The answer was successfully saved.";
        }

        public async Task<string> SetRound2AnswerSurvey(Round2AnswerDto submitAnswer)
        {
            // check limits
            if (submitAnswer.PlayerNum is not (> 0 and < 3))
            {
                var err = "The player number is not valid. Please try again.";
                return err;
            }

            if (submitAnswer.QuestionNum is < 200 or > 299)
            {
                return "The question is not a valid round 2 question. Please try again.";
            }

            if (submitAnswer.TeamNum < 1)
            {
                return "A valid team number is required.";
            }

            if (submitAnswer.AnswerNum is < 0 or > 99)
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
                TeamAnswer = answerText.QuestionAnswer[0..11],
                PlayerNum = submitAnswer.PlayerNum,
                PointAmt = answerText.Ptsposs,
                Updatetime = DateTime.UtcNow
            };

            var recordExists = await _contextGo.Scoring.AnyAsync(s => s.Yevent == submitAnswer.YEvent
                                                        && s.TeamNo == submitAnswer.TeamNum
                                                        && s.QuestionNo == submitAnswer.QuestionNum);

            if (recordExists)
            {
                _contextGo.Scoring.UpdateRange(scoreRecord);
                await _contextGo.SaveChangesAsync();
            }

            if (!recordExists)
            {
                await _contextGo.AddAsync(scoreRecord);
                await _contextGo.SaveChangesAsync();
            }

            return "The answer was successfully saved.";
        }

        public async Task<string> FinalizeRound(string yEvent, int roundNum)
        {
            if (yEvent is null)
            {
                return "No event was specified.";
            }

            if (roundNum is < 1 or > 3)
            {
                return "Incorrect round number.";
            }

            var totalPoints = await _contextGo.Scoring.Where(s => s.RoundNo == roundNum && s.Yevent == yEvent)
                                .GroupBy(s => s.TeamNo)
                                .Select(s => new Round2FinalDto()
                                {
                                    TeamNo = s.Key,
                                    FinalScore = s.Sum(s => s.PointAmt)
                                }).ToListAsync();

            // now we rank and store into the DB. First we remove anything that already exists.

            var scorestoRemove = await _contextGo.Roundresult.Where(r => r.Yevent == yEvent && r.RoundNo == roundNum).ToListAsync();

            if (scorestoRemove.Count > 0)
            {
                _contextGo.RemoveRange(scorestoRemove);
                await _contextGo.SaveChangesAsync();
            }

            // add the new records
            var scorestoAdd = (from s in totalPoints
                               orderby s.FinalScore descending
                               select new Roundresult()
                               {
                                   Yevent = yEvent,
                                   TeamNo = s.TeamNo,
                                   RoundNo = roundNum,
                                   Ptswithbonus = s.FinalScore,
                                   Rnk = (from r in totalPoints
                                          where r.FinalScore > s.FinalScore
                                          select r).Count() + 1
                               }).ToList();

            await _contextGo.Roundresult.AddRangeAsync(scorestoAdd);
            await _contextGo.SaveChangesAsync();


            return "Scores have been finalized in the system.";

        }

        #region Round1

        public async Task<List<Round1EnteredAnswers>> ShowRound1TeamEnteredAnswers(string yEvent, int questionId)
        {
            var returnDto = new List<Round1EnteredAnswers>();

            var submittedAnswer = await _contextGo.UserAnswer
                                    .Where(u => u.RoundNo == 1 && u.QuestionNo == questionId && u.Yevent == yEvent)
                                    .ToListAsync();

            var scoredAnswer = await _contextGo.Scoring.Where(s => s.RoundNo == 1 && s.Yevent == yEvent && s.QuestionNo == questionId)
                                .ToListAsync();

            if (submittedAnswer is null)
            {
                return null;
            }

            foreach (var answer in submittedAnswer)
            {
                var displayAnswer = new Round1EnteredAnswers()
                {
                    TeamNum = answer.TeamNo,
                    QuestionNum = questionId,
                    TextAnswer = answer.TextAnswer,
                    AnswerStatus = scoredAnswer.Any(s => s.TeamNo == answer.TeamNo)
                };
                returnDto.Add(displayAnswer);
            }

            return returnDto;
        }

        // @TODO: fix this.
        public async Task<List<IntroDto>> GetTeamList(string yEvent)
        {
            var rawList = await _contextGo.TeamUser.Where(tu => tu.Yevent == yEvent && tu.TeamNo > 0)
                                                    .OrderBy(tu => tu.TeamNo)
                                                    .ThenBy(tu => tu.PlayerNum)
                                                    .ToListAsync();

            var teamList = await _contextGo.Teamreference.Where(tr => tr.Yevent == yEvent).ToListAsync();

            var rawListPlayer1 = rawList.Where(r => r.PlayerNum == 1).ToList();
            var rawListPlayer2 = rawList.Where(r => r.PlayerNum == 2).ToList();

            var returnDto = (from r1 in rawListPlayer1
                             join r2 in rawListPlayer2
                             on r1.TeamNo equals r2.TeamNo into r2j
                             from r2o in r2j.DefaultIfEmpty()
                             join tl in teamList
                             on r1.TeamNo equals tl.TeamNo
                             select new IntroDto()
                             {
                                 TeamNo = r1.TeamNo,
                                 TeamName = tl.Teamname,
                                 Member1 = r1.PlayerName,
                                 Member2 = r2o is null ? "" : r2o.PlayerName,
                                 Workgroup1 = r1.WorkgroupName,
                                 Workgroup2 = r2o is null ? "" : r2o.WorkgroupName
                             }).ToList();

            return returnDto;

        }

        #endregion

        public async Task<string> GetCurrentEvent()
        {
            var currentEvent = await _contextGo.EventMaster.SingleOrDefaultAsync(e => e.SelEvent == true);
            return currentEvent.Yevent ?? null;
        }

        public async Task<CurrentQuestionDto> GetCurrentQuestion(string yEvent)
        {
            var currentQuestion = await _contextGo.CurrentQuestion
                                        .Where(q => q.YEvent == yEvent)
                                        .OrderByDescending(q => q.QuestionTime)
                                        .Select(q => new CurrentQuestionDto()
                                        {
                                            QuestionNum = q.QuestionNum,
                                            Status = q.Status
                                        }).FirstOrDefaultAsync();

            return currentQuestion is null
                ? new CurrentQuestionDto()
                {
                    QuestionNum = 0,
                    Status = 0
                }
                : currentQuestion;
        }

        public async Task<CurrentQuestionDto> SetCurrentQuestionStatus(string yEvent, int questionId, int status)
        {
            if (questionId < 1)
            {
                return null;
            }

            if (status is < 0 or > 3)
            {
                return null;
            }

            var newQuestionStatus = new CurrentQuestion()
            {
                YEvent = yEvent,
                QuestionNum = questionId,
                Status = status,
                QuestionTime = DateTime.UtcNow
            };

            _contextGo.CurrentQuestion.Add(newQuestionStatus);
            await _contextGo.SaveChangesAsync();

            return new CurrentQuestionDto()
            {
                QuestionNum = questionId,
                Status = status
            };
        }
    }
}
