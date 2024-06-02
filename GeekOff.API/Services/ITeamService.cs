namespace GeekOff.Services
{
    public interface ITeamService
    {
        public Task<List<NewTeamEntry>> GetTeamListAsync(string yEvent);
    }
}
