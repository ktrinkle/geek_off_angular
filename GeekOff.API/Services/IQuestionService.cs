namespace GeekOff.Services
{
    public interface IQuestionService
    {
        Task<List<Round1QuestionControlDto>> GetAllRound1Questions(string yEvent);
        Task<bool> SubmitRound1Answer(string yEvent, int questionId, string answerText, int teamNum);
        Task<List<Round1QuestionDto>> GetRound1QuestionListWithAnswers(string yEvent);
    }
}
