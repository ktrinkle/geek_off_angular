namespace GeekOff.Handlers;

public class GetScoresHandler
{
    public record Request : IRequest<ApiResponse<List<Round23Scores>>>
    {
        public required string YEvent { get; init; } = string.Empty;
        public required int RoundNum { get; init; }
        public required int MaxRnk { get; init; }
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<List<Round23Scores>>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<List<Round23Scores>>> Handle(Request request, CancellationToken token)
        {
            if (request.YEvent == null || request.RoundNum < 2 || request.RoundNum > 3)
            {
                return ApiResponse<List<Round23Scores>>.BadRequest();
            }

            var teamList = await (from rr in _contextGo.Roundresult
                                  join t in _contextGo.Teamreference
                                  on new { rr.TeamNum, rr.Yevent } equals new { t.TeamNum, t.Yevent }
                                  where rr.RoundNum == request.RoundNum - 1
                                  && rr.Yevent == request.YEvent
                                  && rr.Rnk <= request.MaxRnk

                                  orderby rr.Rnk
                                  select new Round23Scores()
                                  {
                                      TeamName = t.Teamname,
                                      TeamNum = t.TeamNum
                                  }).AsNoTracking().ToListAsync(token);

            if (teamList.Count == 0)
            {
                return ApiResponse<List<Round23Scores>>.NotFound();
            }

            var returnList = await _contextGo.Scoring.AsNoTracking()
                                                    .Where(s => s.Yevent == request.YEvent && s.RoundNum == request.RoundNum)
                                                    .GroupBy(s => s.TeamNum)
                                                    .Select(s => new Round23Scores
                                                    {
                                                        TeamNum = s.Key,
                                                        TeamScore = s.Sum(x => (int)x.PointAmt!)
                                                    }).ToListAsync(token);

            // merge the 2
            foreach (var team in teamList)
            {
                var thisTeam = returnList.Find(t => t.TeamNum == team.TeamNum);
                if (thisTeam is not null)
                {
                    team.TeamScore = thisTeam.TeamScore;
                }
            }

            return ApiResponse<List<Round23Scores>>.Success(teamList);
        }
    }
}
