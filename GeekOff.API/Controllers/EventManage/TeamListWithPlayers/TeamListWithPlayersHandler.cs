namespace GeekOff.Handlers;

public class TeamListWithPlayersHandler
{
    public class Request : IRequest<ApiResponse<List<IntroDto>>>
    {
        public string YEvent { get; set; } = string.Empty;
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<List<IntroDto>>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<List<IntroDto>>> Handle(Request request, CancellationToken token)
        {
            if (string.IsNullOrEmpty(request.YEvent))
            {
                return ApiResponse<List<IntroDto>>.NotFound();
            }

            var rawList = await _contextGo.TeamUser.Where(tu => tu.Yevent == request.YEvent && tu.TeamNum > 0)
                                                    .OrderBy(tu => tu.TeamNum)
                                                    .ThenBy(tu => tu.PlayerNum)
                                                    .ToListAsync();

            if (rawList.Count == 0)
            {
                return ApiResponse<List<IntroDto>>.NotFound();
            }

            var teamList = await _contextGo.Teamreference.Where(tr => tr.Yevent == request.YEvent).ToListAsync();

            var rawListPlayer1 = rawList.Where(r => r.PlayerNum == 1).ToList();
            var rawListPlayer2 = rawList.Where(r => r.PlayerNum == 2).ToList();

            var returnDto = (from r1 in rawListPlayer1
                             join r2 in rawListPlayer2
                             on r1.TeamNum equals r2.TeamNum into r2j
                             from r2o in r2j.DefaultIfEmpty()
                             join tl in teamList
                             on r1.TeamNum equals tl.TeamNum
                             select new IntroDto()
                             {
                                 TeamNum = r1.TeamNum,
                                 TeamName = tl.Teamname,
                                 Member1 = r1.PlayerName,
                                 Member2 = r2o is null ? "" : r2o.PlayerName,
                                 Workgroup1 = r1.WorkgroupName,
                                 Workgroup2 = r2o is null ? "" : r2o.WorkgroupName
                             }).ToList();

            return ApiResponse<List<IntroDto>>.Success(returnDto);
        }
    }
}
