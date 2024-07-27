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

        public async Task<Round2BoardDto> GetRound2DisplayBoard(string yEvent, int teamNum)
        {
            // looks up current team from DB for state
            // calculates total and places in DTO

            var currentScore = await _contextGo.Scoring.Where(s => s.RoundNum == 2 && s.TeamNum == teamNum && s.Yevent == yEvent)
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
                TeamNum = teamNum,
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

        public async Task<List<Round23Scores>> GetRound23Scores(string yEvent, int roundNum, int maxRnk)
        {
            if (yEvent == null || roundNum < 2 || roundNum > 3)
            {
                return null;
            }

            var teamList = await (from rr in _contextGo.Roundresult
                                  join t in _contextGo.Teamreference
                                  on new { rr.TeamNum, rr.Yevent } equals new { t.TeamNum, t.Yevent }
                                  where rr.RoundNum == roundNum - 1
                                  && rr.Yevent == yEvent
                                  && rr.Rnk <= maxRnk

                                  orderby rr.Rnk
                                  select new Round23Scores()
                                  {
                                      TeamName = t.Teamname,
                                      TeamNum = t.TeamNum
                                  }).ToListAsync();

            // the join was nasty and required a view, so this way avoids that.

            var returnList = await _contextGo.Scoring.Where(s => s.Yevent == yEvent && s.RoundNum == roundNum)
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
