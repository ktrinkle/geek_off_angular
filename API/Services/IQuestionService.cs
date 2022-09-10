namespace GeekOff.Services
{
    public interface IQuestionService
    {
        Task<List<Round1QuestionDisplay>> GetRound1QuestionAsync(string yEvent);
        Task<Round1QuestionDto> GetRound1QuestionWithAnswer(string yEvent, int questionNum);
        Task<List<Round1QuestionControlDto>> GetAllRound1Questions(string yEvent);
        Task<bool> SubmitRound1Answer(string yEvent, int questionId, string answerText, int teamNum);
        Task<List<Round1QuestionDto>> GetRound1QuestionListWithAnswers(string yEvent);
    }
}
