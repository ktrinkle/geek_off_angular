namespace GeekOff.Handlers;

public class RoundThreeGeekOMaticScoreHandler
{
    public record Request : IRequest<ApiResponse<List<Round3GeekOMaticScores>>>
    {
        public string YEvent { get; set; } = string.Empty;
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<List<Round3GeekOMaticScores>>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<List<Round3GeekOMaticScores>>> Handle(Request request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.YEvent))
            {
            return ApiResponse<List<Round3GeekOMaticScores>>.BadRequest();
            }

            var round3Teams = await _contextGo.Roundresult
                                .Where(tr => tr.RoundNum == 2 && tr.Rnk < 4 && tr.Yevent == request.YEvent)
                                .AsNoTracking()
                                .OrderBy(tr => tr.Rnk)
                                .Select(tr => new {tr.TeamNum, tr.Rnk}).ToListAsync(cancellationToken);

            var round3Scoring = await _contextGo.Scoring
                                .Where(tr => tr.RoundNum == 3 && tr.Yevent == request.YEvent)
                                .AsNoTracking()
                                .GroupBy(s => s.TeamNum)
                                .Select(s => new
                                {
                                    TeamNum = s.Key,
                                    TeamScore = s.Sum(s => s.PointAmt)
                                })
                                .ToListAsync(cancellationToken);

            var round3Return = round3Teams.Join(TeamColorConstants.TeamColors,
                                    r3 => r3.Rnk,
                                    tc => tc.Rnk,
                                    (r3, tc) => new Round3GeekOMaticScores()
                                        {
                                            Rnk = tc.Rnk,
                                            TeamNum = r3.TeamNum,
                                            TeamColor = tc.Color
                                        }
                                    ).ToList();

            foreach( var round3Team in round3Return )
            {
                var teamScoreDto = round3Scoring.Find(s => round3Team.TeamNum == s.TeamNum);
                var scoreNumeric = teamScoreDto is not null ? teamScoreDto.TeamScore ?? 0 : 0;

                round3Team.TeamScore = (scoreNumeric < 10000 ? scoreNumeric : 9999).ToString().PadLeft(4, ' ');
            }

            return round3Return.Count != 0 ? ApiResponse<List<Round3GeekOMaticScores>>.Success(round3Return) : ApiResponse<List<Round3GeekOMaticScores>>.NotFound();
        }
    }
}
