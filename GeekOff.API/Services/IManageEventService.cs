namespace GeekOff.Services
{
    public interface IManageEventService
    {
        Task<string> SetRound3Answer(List<Round3AnswerDto> round3Answers);
    }
}
