namespace GeekOff.Handlers;

public class RoundThreeTeamListHandler
{
    public record Request : IRequest<ApiResponse<List<IntroDto>>>
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
                return ApiResponse<List<IntroDto>>.BadRequest();
             }

            var round3Teams = await (from rr in _contextGo.Roundresult
                                     join tr in _contextGo.Teamreference
                                     on new { rr.TeamNum, rr.Yevent } equals new { tr.TeamNum, tr.Yevent }
                                     where rr.RoundNum == 2
                                     && rr.Yevent == request.YEvent
                                     && rr.Rnk < 4
                                     orderby rr.Rnk
                                     select new IntroDto()
                                     {
                                         TeamName = tr.Teamname,
                                         TeamNum = tr.TeamNum
                                     }).AsNoTracking().ToListAsync(token);

            return round3Teams.Count == 0 ? ApiResponse<List<IntroDto>>.NotFound() : ApiResponse<List<IntroDto>>.Success(round3Teams);
        }
    }
}
