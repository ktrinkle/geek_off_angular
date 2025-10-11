namespace GeekOff.Handlers;

public class RoundTwoDisplayBoardHandler
{
    public record Request : IRequest<ApiResponse<Round2BoardDto>>
    {
        public string YEvent { get; init; } = string.Empty;
        public int TeamNum { get; init; }
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<Round2BoardDto>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<Round2BoardDto>> Handle(Request request, CancellationToken token)
        {
            // looks up current team from DB for state
            // calculates total and places in DTO

            var currentScore = await _contextGo.Scoring.AsNoTracking().Where(s => s.RoundNum == 2 && s.TeamNum == request.TeamNum && s.Yevent == request.YEvent)
                                .OrderBy(s => s.QuestionNum).OrderBy(s => s.PlayerNum).ToListAsync(token);

            if (currentScore.Count == 0)
            {
                return ApiResponse<Round2BoardDto>.NotFound();
            }

            var allQuestion = await _contextGo.QuestionAns.Where(q => q.RoundNum == 2 && q.QuestionNum < 206 && q.Yevent == request.YEvent)
                                .AsNoTracking().OrderBy(q => q.QuestionNum).ToListAsync(token);

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
                        Answer = (playerScore.TeamAnswer ?? string.Empty).ToUpper(),
                        Score = (int)playerScore.PointAmt!
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
                    Answer = (playerScore.TeamAnswer ?? string.Empty).ToUpper(),
                    Score = (int)playerScore.PointAmt!
                };
                totalScore += result.Score;
                player2.Add(result);
            }

            var returnResult = new Round2BoardDto()
            {
                TeamNum = request.TeamNum,
                Player1Answers = player1,
                Player2Answers = player2,
                FinalScore = totalScore
            };

            return ApiResponse<Round2BoardDto>.Success(returnResult);
        }
    }
}
