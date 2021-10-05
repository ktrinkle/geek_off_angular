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
    public class ScoreService : IScoreService
    {
        private readonly contextGo _contextGo;
        private readonly ILogger<ScoreService> _logger;
        public ScoreService(ILogger<ScoreService> logger, contextGo context)
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

            foreach(Scoring playerScore in player1Result)
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

            foreach(Scoring playerScore in player2Result)
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

            var teamList = await _contextGo.Teamreference.Where(tr => tr.Yevent == yEvent)
                                .Select(tr => new Round1Scores() {
                                    TeamName = tr.Teamname,
                                    TeamNum = tr.TeamNo,
                                    Q = _contextGo.Scoring.Where(s => s.RoundNo == 1 && s.TeamNo == tr.TeamNo && s.Yevent == tr.Yevent)
                                        .Select(s => new Round1ScoreDetail() {
                                            QuestionId = s.QuestionNo,
                                            QuestionScore = s.PointAmt
                                        }).ToList()
                                }).ToListAsync();

            // now we need to calc the TeamScore and Rnk
            
            foreach (Round1Scores team in teamList)
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
                                  on new {rr.TeamNo, rr.Yevent} equals new {t.TeamNo, t.Yevent}
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
                                                    .Select(s => new Round23Scores {
                                                        TeamNo = s.Key,
                                                        TeamScore = s.Sum(x => (int)x.PointAmt)
                                                    }).ToListAsync();

            // merge the 2
            foreach (Round23Scores team in teamList)
            {
                var thisTeam = returnList.Find(t => t.TeamNo == team.TeamNo);
                if (thisTeam is not null)
                {
                    team.TeamScore = thisTeam.TeamScore;
                }
            }

            return teamList;

        }
    }
}
