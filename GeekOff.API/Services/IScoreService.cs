namespace GeekOff.Services
{
    public interface IScoreService
    {
        Task<List<Round2Answers>> GetFirstPlayersAnswers(string yEvent, int teamNum);
    }
}
