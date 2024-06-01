namespace GeekOff.Services
{
    public interface ITeamService
    {
        public Task<NewTeamEntry> AddNewEventTeamAsync(string yEvent, string? teamName);
        public Task<ApiResponse> MoveTeamNumberAsync(string yEvent, int teamNum, int newTeamNum);
        public Task<List<NewTeamEntry>> GetTeamListAsync(string yEvent);
    }
}
