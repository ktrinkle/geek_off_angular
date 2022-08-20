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

        public async Task<Round2BoardDto> GetRound2DisplayBoard(string yEvent, int TeamNum)
        {
            // looks up current team from DB for state
            // calculates total and places in DTO

            var currentScore = await _contextGo.Scoring.Where(s => s.RoundNum == 2 && s.TeamNum == TeamNum && s.Yevent == yEvent)
                                .OrderBy(s => s.QuestionNum).OrderBy(s => s.PlayerNum).ToListAsync();

            if (currentScore is null)
            {
                return null;
            }

            var allQuestion = await _contextGo.QuestionAns.Where(q => q.RoundNum == 2 && q.QuestionNum < 206 && q.Yevent == yEvent)
                                .OrderBy(q => q.QuestionNum).ToListAsync();

            var player1Result = currentScore.Where(s => s.PlayerNum == 1).OrderBy(s => s.QuestionNum);
            var player2Result = currentScore.Where(s => s.PlayerNum == 2).OrderBy(s => s.QuestionNum);

            var player1 = new List<Round2Answers>();
            var player2 = new List<Round2Answers>();
            var totalScore = new int();

            if (player1Result.Any())
            {
                foreach (var playerScore in player1Result)
                {
                    var result = new Round2Answers()
                    {
                        QuestionNum = playerScore.QuestionNum,
                        Answer = playerScore.TeamAnswer.ToUpper(),
                        Score = (int)playerScore.PointAmt
                    };
                    totalScore += result.Score;
                    player1.Add(result);
                }
            }

            // this will always give us something if no answers are present so we keep the display.
            if (!player1Result.Any())
            {
                foreach (var emptyScore in allQuestion)
                {
                    var result = new Round2Answers()
                    {
                        QuestionNum = emptyScore.QuestionNum
                    };
                    player1.Add(result);
                    player2.Add(result);
                };
                totalScore = 0;
            }

            // by design we cannot have a player 2 without a player 1.
            foreach (var playerScore in player2Result)
            {
                var result = new Round2Answers()
                {
                    QuestionNum = playerScore.QuestionNum,
                    Answer = playerScore.TeamAnswer.ToUpper(),
                    Score = (int)playerScore.PointAmt
                };
                totalScore += result.Score;
                player2.Add(result);
            }

            var returnResult = new Round2BoardDto()
            {
                TeamNum = TeamNum,
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
                                    TeamNum = tr.TeamNum,
                                    Bonus = tr.Dollarraised >= 200 ? 10 : tr.Dollarraised > 100 ? (int)(tr.Dollarraised - 100) / 10 : 0,
                                    Q = (from q in _contextGo.QuestionAns
                                         join s in _contextGo.Scoring
                                         on new { q.RoundNum, q.QuestionNum, q.Yevent, tr.TeamNum }
                                         equals new { s.RoundNum, s.QuestionNum, s.Yevent, s.TeamNum } into sq
                                         from sqi in sq.DefaultIfEmpty()
                                         where q.RoundNum == 1 && q.Yevent == tr.Yevent
                                         select new Round1ScoreDetail()
                                         {
                                             QuestionId = q.QuestionNum,
                                             QuestionScore = sqi.PointAmt
                                         }).ToList()
                                }).ToListAsync();

            // now we need to calc the TeamScore. Rnk is not used here.
            // team substring not liked in EF query. Forgot to add the bonus, whoops

            foreach (var team in teamList)
            {
                team.TeamScore = team.Q.Sum(s => s.QuestionScore) + team.Bonus;
            }

            return teamList;

        }

        public async Task<List<Round23Scores>> GetRound23Scores(string yEvent, int RoundNum, int maxRnk)
        {
            if (yEvent == null || RoundNum < 2 || RoundNum > 3)
            {
                return null;
            }

            var teamList = await (from rr in _contextGo.Roundresult
                                  join t in _contextGo.Teamreference
                                  on new { rr.TeamNum, rr.Yevent } equals new { t.TeamNum, t.Yevent }
                                  where rr.RoundNum == RoundNum - 1
                                  && rr.Yevent == yEvent
                                  && rr.Rnk <= maxRnk

                                  orderby rr.Rnk
                                  select new Round23Scores()
                                  {
                                      TeamName = t.Teamname,
                                      TeamNum = t.TeamNum
                                  }).ToListAsync();

            // the join was nasty and required a view, so this way avoids that.

            var returnList = await _contextGo.Scoring.Where(s => s.Yevent == yEvent && s.RoundNum == RoundNum)
                                                    .GroupBy(s => s.TeamNum)
                                                    .Select(s => new Round23Scores
                                                    {
                                                        TeamNum = s.Key,
                                                        TeamScore = s.Sum(x => (int)x.PointAmt)
                                                    }).ToListAsync();

            // merge the 2
            foreach (var team in teamList)
            {
                var thisTeam = returnList.Find(t => t.TeamNum == team.TeamNum);
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

            var submittedAnswers = await _contextGo.UserAnswer.Where(u => u.QuestionNum == questionId && u.Yevent == yEvent).ToListAsync();
            var questionInfo = await _contextGo.QuestionAns.FirstOrDefaultAsync(u => u.QuestionNum == questionId && u.Yevent == yEvent);

            if (questionInfo is null)
            {
                return "Unable to load question.";
            }

            var submittedTeams = submittedAnswers.Select(t => t.TeamNum).ToList();

            var correctAnswer = questionInfo.CorrectAnswer;
            var scoring = new List<Scoring>();

            // remove existing answers for the auto-score process
            var answersToRemove = await _contextGo.Scoring.Where(s => s.Yevent == yEvent && s.RoundNum == 1 && submittedTeams.Contains(s.TeamNum) && s.QuestionNum == questionId).ToListAsync();

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
                        TeamNum = answer.TeamNum,
                        RoundNum = 1,
                        QuestionNum = answer.QuestionNum,
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

            var scoring = await _contextGo.Scoring.Where(s => s.Yevent == yEvent && s.TeamNum == teamNum && s.QuestionNum == questionId).ToListAsync();

            if (scoring.Count > 0)
            {
                // remove the entry since it's a flip back
                _contextGo.Scoring.RemoveRange(scoring);
                await _contextGo.SaveChangesAsync();

                return false;
            }

            if (scoring.Count == 0)
            {
                var teamAnswer = await _contextGo.UserAnswer.FirstOrDefaultAsync(u => u.Yevent == yEvent && u.QuestionNum == questionId && u.TeamNum == teamNum);

                if (teamAnswer is not null)
                {
                    // add a new entry
                    var teamScore = new Scoring()
                    {
                        Yevent = yEvent,
                        TeamNum = teamNum,
                        RoundNum = 1,
                        QuestionNum = questionId,
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
            var playerList = await _contextGo.Scoring.Where(x => x.RoundNum == 2 &&
                                                     x.TeamNum == teamNum && x.Yevent == yEvent)
                                              .FirstOrDefaultAsync();

            if (playerList.PlayerNum is not null)
            {
                var answers = await _contextGo.Scoring.Where(x => x.RoundNum == 2 &&
                                                       x.TeamNum == teamNum &&
                                                       x.PlayerNum == playerList.PlayerNum)
                                                .Select(x => new Round2Answers {
                                                                    QuestionNum = x.QuestionNum,
                                                                    Answer = x.TeamAnswer,
                                                                    Score = (int)x.PointAmt
                                                }).ToListAsync();
                return answers;
            }

            return null;
        }
    }
}
