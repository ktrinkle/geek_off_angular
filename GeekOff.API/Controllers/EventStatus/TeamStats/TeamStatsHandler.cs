namespace GeekOff.Handlers;

public class TeamStatsHandler
{
    public class Request : IRequest<ApiResponse<List<Round23Scores>>>
    {
        public string YEvent { get; set; } = string.Empty;
    }

    private sealed record TempReturnList
    {
        public int TeamNum { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public int RoundNum { get; set; }
        public int? TeamScore { get; set; }
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<List<Round23Scores>>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<List<Round23Scores>>> Handle(Request request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.YEvent))
            {
                return ApiResponse<List<Round23Scores>>.NotFound();
            }

            var teamList = await (from rr in _contextGo.Roundresult
                        join t in _contextGo.Teamreference
                        on new { rr.TeamNum, rr.Yevent } equals new { t.TeamNum, t.Yevent }
                        where rr.Yevent == request.YEvent

                        orderby rr.RoundNum descending, rr.Rnk
                        select new TempReturnList()
                        {
                            TeamName = t.Teamname ?? string.Empty,
                            TeamNum = t.TeamNum,
                            TeamScore = (int?)rr.Ptswithbonus,
                            RoundNum = rr.RoundNum
                        }).AsNoTracking().ToListAsync(cancellationToken);

            if (teamList.Count == 0)
            {
                return ApiResponse<List<Round23Scores>>.NotFound();
            }

            var returnBaseResult = new List<Round23Scores>();

            // this is dependent upon the sort above.

            foreach(var team in teamList)
            {
                if (!returnBaseResult.Exists(rr => rr.TeamNum == team.TeamNum))
                {
                    returnBaseResult.Add(new Round23Scores()
                    {
                        TeamNum = team.TeamNum,
                        TeamName = team.TeamName ?? string.Empty,
                        TeamScore = team.TeamScore,
                        Rnk = returnBaseResult.Count+1
                    });
                }
            }

            return ApiResponse<List<Round23Scores>>.Success(returnBaseResult);
        }
    }
}
