namespace GeekOff.Handlers;

public class TeamListHandler
{
    public class Request : IRequest<ApiResponse<List<NewTeamEntry>>>
    {
        public string YEvent { get; set; } = string.Empty;
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<List<NewTeamEntry>>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<List<NewTeamEntry>>> Handle(Request request, CancellationToken token)
        {
            if (request.YEvent is null)
            {
                return ApiResponse<List<NewTeamEntry>>.NotFound();
            }

            var teamList = await _contextGo.Teamreference
                .Where(tr => tr.Yevent == request.YEvent)
                .Select(tr => new NewTeamEntry()
                {
                    TeamNum = tr.TeamNum,
                    TeamGuid = tr.TeamGuid,
                    TeamName = tr.Teamname
                })
                .ToListAsync(cancellationToken: token);

            return ApiResponse<List<NewTeamEntry>>.Success(teamList);
        }
    }
}
