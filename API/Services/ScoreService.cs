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
    public class ScoreService : IScoreService
    {
        private readonly ContextGo _contextGo;
        private readonly ILogger<ScoreService> _logger;
        public ScoreService(ILogger<ScoreService> logger, ContextGo context)
        {
            _logger = logger;
            _contextGo = context;
        }

        public async Task<Round2BoardDto> GetRound2DisplayBoard(string yEvent, int teamNo)
        {
            // looks up current team from DB for state
            // calculates total and places in DTO

            var currentScore = await _contextGo.Scoring.Where(s => s.RoundNo == 2 && s.TeamNo == teamNo && s.Yevent == yEvent)
                                .OrderBy(s => s.QuestionNo).OrderBy(s => s.PlayerNum).ToListAsync();

            if (currentScore is null)
            {
                return null;
            }

            var player1Result = currentScore.Where(s => s.PlayerNum == 1).OrderBy(s => s.QuestionNo);
            var player2Result = currentScore.Where(s => s.PlayerNum == 2).OrderBy(s => s.QuestionNo);

            var player1 = new List<Round2Answers>();
            var player2 = new List<Round2Answers>();
            var totalScore = new int();

            foreach (var playerScore in player1Result)
            {
                var result = new Round2Answers()
                {
                    QuestionNum = playerScore.QuestionNo,
                    Answer = playerScore.TeamAnswer.ToUpper(),
                    Score = (int)playerScore.PointAmt
                };
                totalScore += result.Score;
                player1.Add(result);
            }

            foreach (var playerScore in player2Result)
            {
                var result = new Round2Answers()
                {
                    QuestionNum = playerScore.QuestionNo,
                    Answer = playerScore.TeamAnswer.ToUpper(),
                    Score = (int)playerScore.PointAmt
                };
                totalScore += result.Score;
                player2.Add(result);
            }

            var returnResult = new Round2BoardDto()
            {
                TeamNo = teamNo,
                Player1Answers = player1,
                Player2Answers = player2,
                FinalScore = totalScore
            };

            return returnResult;
        }

        public async Task<List<Round1Scores>> GetRound1Scores(string yEvent)
        {
            if (yEvent == null)
            {
                return null;
            }

            // question and team needs
            var teamList = await _contextGo.Teamreference.Where(tr => tr.Yevent == yEvent)
                                .Select(tr => new Round1Scores()
                                {
                                    TeamName = tr.Teamname.ToUpper(),
                                    TeamNum = tr.TeamNo,
                                    Bonus = tr.Dollarraised >= 200 ? 10 : tr.Dollarraised > 100 ? (int)(tr.Dollarraised - 100) / 10 : 0,
                                    Q = (from q in _contextGo.QuestionAns
                                         join s in _contextGo.Scoring
                                         on new { q.RoundNo, q.QuestionNo, q.Yevent, tr.TeamNo }
                                         equals new { s.RoundNo, s.QuestionNo, s.Yevent, s.TeamNo } into sq
                                         from sqi in sq.DefaultIfEmpty()
                                         where q.RoundNo == 1 && q.Yevent == tr.Yevent
                                         select new Round1ScoreDetail()
                                         {
                                             QuestionId = q.QuestionNo,
                                             QuestionScore = sqi.PointAmt
                                         }).ToList()
                                }).ToListAsync();

            // now we need to calc the TeamScore. Rnk is not used here.
            // team substring not liked in EF query

            foreach (var team in teamList)
            {
                team.TeamScore = team.Q.Sum(s => s.QuestionScore);
            }

            return teamList;

        }

        public async Task<List<Round23Scores>> GetRound23Scores(string yEvent, int roundNo)
        {
            if (yEvent == null || roundNo < 2 || roundNo > 3)
            {
                return null;
            }

            var teamList = await (from rr in _contextGo.Roundresult
                                  join t in _contextGo.Teamreference
                                  on new { rr.TeamNo, rr.Yevent } equals new { t.TeamNo, t.Yevent }
                                  where rr.RoundNo == roundNo - 1
                                  && rr.Yevent == yEvent
                                  select new Round23Scores()
                                  {
                                      TeamName = t.Teamname,
                                      TeamNo = t.TeamNo
                                  }).ToListAsync();

            // the join was nasty and required a view, so this way avoids that.

            var returnList = await _contextGo.Scoring.Where(s => s.Yevent == yEvent && s.RoundNo == roundNo)
                                                    .GroupBy(s => s.TeamNo)
                                                    .Select(s => new Round23Scores
                                                    {
                                                        TeamNo = s.Key,
                                                        TeamScore = s.Sum(x => (int)x.PointAmt)
                                                    }).ToListAsync();

            // merge the 2
            foreach (var team in teamList)
            {
                var thisTeam = returnList.Find(t => t.TeamNo == team.TeamNo);
                if (thisTeam is not null)
                {
                    team.TeamScore = thisTeam.TeamScore;
                }
            }

            return teamList;

        }

        public async Task<string> ScoreAnswerAutomatic(string yEvent, int questionId)
        {
            if (yEvent is null)
            {
                return "Invalid event.";
            }

            if (questionId is < 1 or > 99)
            {
                return "Invalid question.";
            }

            var submittedAnswers = await _contextGo.UserAnswer.Where(u => u.QuestionNo == questionId && u.Yevent == yEvent).ToListAsync();
            var questionInfo = await _contextGo.QuestionAns.FirstOrDefaultAsync(u => u.QuestionNo == questionId && u.Yevent == yEvent);

            if (questionInfo is null)
            {
                return "Unable to load question.";
            }

            var submittedTeams = submittedAnswers.Select(t => t.TeamNo).ToList();

            var correctAnswer = questionInfo.CorrectAnswer;
            var scoring = new List<Scoring>();

            // remove existing answers for the auto-score process
            var answersToRemove = await _contextGo.Scoring.Where(s => s.Yevent == yEvent && s.RoundNo == 1 && submittedTeams.Contains(s.TeamNo) && s.QuestionNo == questionId).ToListAsync();

            if (answersToRemove is not null)
            {
                _contextGo.Scoring.RemoveRange(answersToRemove);
                await _contextGo.SaveChangesAsync();
            }

            foreach (var answer in submittedAnswers)
            {
                if (answer.TextAnswer.ToLower() == correctAnswer.ToLower())
                {
                    var teamScore = new Scoring()
                    {
                        Yevent = answer.Yevent,
                        TeamNo = answer.TeamNo,
                        RoundNo = 1,
                        QuestionNo = answer.QuestionNo,
                        TeamAnswer = answer.TextAnswer,
                        PointAmt = 10,
                        Updatetime = answer.AnswerTime
                    };

                    scoring.Add(teamScore);
                }
            }

            await _contextGo.AddRangeAsync(scoring);
            await _contextGo.SaveChangesAsync();

            return "Auto-scoring complete.";
        }

        public async Task<bool> ScoreAnswerManual(string yEvent, int questionId, int teamNum)
        {
            // we don't need to look at the answer since a human has done so. But we are flipping the score if it exists.

            var scoring = await _contextGo.Scoring.Where(s => s.Yevent == yEvent && s.TeamNo == teamNum && s.QuestionNo == questionId).ToListAsync();

            if (scoring.Count > 0)
            {
                // remove the entry since it's a flip back
                _contextGo.Scoring.RemoveRange(scoring);
                await _contextGo.SaveChangesAsync();

                return false;
            }

            if (scoring.Count == 0)
            {
                var teamAnswer = await _contextGo.UserAnswer.FirstOrDefaultAsync(u => u.Yevent == yEvent && u.QuestionNo == questionId && u.TeamNo == teamNum);

                if (teamAnswer is not null)
                {
                    // add a new entry
                    var teamScore = new Scoring()
                    {
                        Yevent = yEvent,
                        TeamNo = teamNum,
                        RoundNo = 1,
                        QuestionNo = questionId,
                        TeamAnswer = teamAnswer.TextAnswer,
                        PointAmt = 10,
                        Updatetime = teamAnswer.AnswerTime
                    };

                    await _contextGo.Scoring.AddAsync(teamScore);
                    await _contextGo.SaveChangesAsync();

                    return true;
                }

            }

            // this should never be hit.
            return false;
        }

        public async Task<List<Round2Answers>> GetFirstPlayersAnswers(string yEvent, int teamNum)
        {
            var playerNum = _contextGo.Scoring.Where(x => x.RoundNo == 2 &&
                                                     x.TeamNo == teamNum)
                                              .FirstOrDefault().PlayerNum;

            if (playerNum is not null)
            {
                var answers = await _contextGo.Scoring.Where(x => x.RoundNo == 2 &&
                                                       x.TeamNo == teamNum &&
                                                       x.PlayerNum == playerNum)
                                                .Select(x => new Round2Answers {
                                                                    QuestionNum = x.QuestionNo,
                                                                    Answer = x.TeamAnswer,
                                                                    Score = (int)x.PointAmt
                                                }).ToListAsync();
                return answers;
            }

            return null;
        }
    }
}
