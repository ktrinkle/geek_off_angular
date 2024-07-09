namespace GeekOff.Services
{
    public interface IManageEventService
    {
        Task<List<Round2SurveyList>> GetRound2SurveyMaster(string yEvent);
        Task<List<Round2SurveyList>> GetRound2QuestionList(string yEvent);
        Task<string> SetRound2AnswerText(Round2AnswerDto submitAnswer);
        Task<string> SetRound2AnswerSurvey(Round2AnswerDto submitAnswer);
        Task<string> FinalizeRound(string yEvent, int roundNum);
        Task<List<Round1EnteredAnswers>> ShowRound1TeamEnteredAnswers(string yEvent, int questionId);
        Task<CurrentQuestionDto> SetCurrentQuestionStatus(string yEvent, int questionId, int status);
        Task<string> SetRound3Answer(List<Round3AnswerDto> round3Answers);
        Task<List<Round3QuestionDto>> GetRound3Master(string yEvent);
        Task<List<IntroDto>> GetRound3Teams(string yEvent);
    }
}
