namespace GeekOff.Services
{
    public interface IManageEventService
    {
        Task<string> SetRound3Answer(List<Round3AnswerDto> round3Answers);
        Task<List<Round3QuestionDto>> GetRound3Master(string yEvent);
        Task<List<IntroDto>> GetRound3Teams(string yEvent);
    }
}
