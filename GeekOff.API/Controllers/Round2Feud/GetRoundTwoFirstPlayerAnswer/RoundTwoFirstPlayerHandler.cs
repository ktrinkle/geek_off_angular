namespace GeekOff.Handlers;

public class RoundTwoFirstPlayerHandler
{
    public record Request : IRequest<ApiResponse<List<Round2Answers>>>
    {
        public string YEvent { get; set; } = string.Empty;
        public int TeamNum { get; set; }
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<List<Round2Answers>>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<List<Round2Answers>>> Handle(Request request, CancellationToken token)
        {
            if (string.IsNullOrEmpty(request.YEvent) || request.TeamNum < 1 || request.TeamNum > 999)
            {
                return ApiResponse<List<Round2Answers>>.BadRequest();
            }

            var playerList = await _contextGo.Scoring.AsNoTracking().AnyAsync(x => x.RoundNum == 2
                                                            && x.TeamNum == request.TeamNum
                                                            && x.Yevent == request.YEvent, token);

            if (!playerList)
            {
                return ApiResponse<List<Round2Answers>>.NotFound();
            }

            var answers = await _contextGo.Scoring.AsNoTracking()
                                                .Where(x => x.RoundNum == 2
                                                       && x.TeamNum == request.TeamNum
                                                       && x.PlayerNum == 1)
                                                .Select(x => new Round2Answers {
                                                                    QuestionNum = x.QuestionNum,
                                                                    Answer = x.TeamAnswer ?? string.Empty,
                                                                    Score = (int)x.PointAmt!
                                                }).ToListAsync(token);

            return ApiResponse<List<Round2Answers>>.Success(answers);
        }
    }
}
