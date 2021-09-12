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

        public async Task<Round2BoardDto> GetRound2Scoreboard()
        {
            // looks up current team from DB for state
            // calculates total and places in DTO

            var currentTeam = await _contextGo.CurrentTeam.FirstOrDefaultAsync(r => r.RoundNo == 2);

            if (currentTeam is null)
            {
                return null;
            }

            var currentScore = await _contextGo.Scoring.Where(s => s.RoundNo == 2 && s.TeamNo == currentTeam.TeamNo)
                                .OrderBy(s => s.QuestionNo).OrderBy(s => s.PlayerNum).ToListAsync();

            if (currentScore is null)
            {
                return null;
            }

            var player1Result = currentScore.Where(s => s.PlayerNum == 1);
            var player2Result = currentScore.Where(s => s.PlayerNum == 2);

            var player1 = new List<Round2Answers>();
            var player2 = new List<Round2Answers>();
            var totalScore = new int();

            foreach(Scoring playerScore in player1Result)
            {
                var result = new Round2Answers() 
                {
                    QuestionNo = playerScore.QuestionNo,
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
                    QuestionNo = playerScore.QuestionNo,
                    Answer = playerScore.TeamAnswer.ToUpper(),
                    Score = (int)playerScore.PointAmt
                };
                totalScore += result.Score;
                player2.Add(result);
            }

            var returnResult = new Round2BoardDto()
            {
                TeamNo = currentTeam.TeamNo,
                Player1Answers = player1,
                Player2Answers = player2,
                FinalScore = totalScore
            };

            return returnResult;
        }
    }
}
