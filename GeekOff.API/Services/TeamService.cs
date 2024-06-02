namespace GeekOff.Services
{
    public class TeamService : ITeamService
    {
        private readonly ContextGo _contextGo;
        private readonly ILogger<TeamService> _logger;
        public TeamService(ILogger<TeamService> logger, ContextGo context)
        {
            _logger = logger;
            _contextGo = context;
        }

        public async Task<List<NewTeamEntry>> GetTeamListAsync(string yEvent)
        {
            if (yEvent is null)
            {
                return null;
            }

            var teamList = await _contextGo.Teamreference.Where(tr => tr.Yevent == yEvent).Select(tr => new NewTeamEntry() {
                TeamNum = tr.TeamNum,
                TeamGuid = tr.TeamGuid,
                SuccessInd = true,
                TeamName = tr.Teamname
            }).ToListAsync();

            return teamList;
        }

    }
}
