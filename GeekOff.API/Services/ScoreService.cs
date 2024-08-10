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
