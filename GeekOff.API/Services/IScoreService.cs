namespace GeekOff.Services
{
    public interface IScoreService
    {
        Task<Round2BoardDto> GetRound2DisplayBoard(string yEvent, int teamNum);
        Task<List<Round1Scores>> GetRound1Scores(string yEvent);
        Task<List<Round23Scores>> GetRound23Scores(string yEvent, int roundNum, int maxRnk);
        Task<List<Round2Answers>> GetFirstPlayersAnswers(string yEvent, int teamNum);
    }
}
