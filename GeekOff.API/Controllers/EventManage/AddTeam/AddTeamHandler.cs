namespace GeekOff.Handlers;

public class AddTeamHandler
{
    public class Request : IRequest<ApiResponse<NewTeamEntry>>
    {
        public string YEvent { get; set; } = string.Empty;
        public string TeamName { get; init; } = string.Empty;
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<NewTeamEntry>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<NewTeamEntry>> Handle(Request request, CancellationToken token)
        {
            // check the DB to see highest team number. Note, we don't reuse team numbers.
            var teamList = await _contextGo.Teamreference.Where(tr=>tr.Yevent == request.YEvent)
                    .MaxAsync(tr => (int?)tr.TeamNum) ?? 0;

            var maxTeamNum = 0;

            if (teamList >= 0)
            {
                maxTeamNum = teamList + 1;
            }

            var newTeamDb = new Teamreference() {
                Yevent = request.YEvent,
                TeamNum = maxTeamNum,
                Teamname = request.TeamName,
                TeamGuid = Guid.NewGuid()
            };

            await _contextGo.AddAsync(newTeamDb, token);
            await _contextGo.SaveChangesAsync(token);

            var teamReturn = new NewTeamEntry()
            {
                TeamNum = newTeamDb.TeamNum,
                TeamGuid = newTeamDb.TeamGuid
            };

            return ApiResponse<NewTeamEntry>.Success(teamReturn);
        }
    }
}
