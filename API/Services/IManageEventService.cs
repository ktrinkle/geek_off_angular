using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeekOff.Models;

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
        Task<List<IntroDto>> GetTeamList(string yEvent);
        Task<string> GetCurrentEvent();
        Task<CurrentQuestionDto> GetCurrentQuestion(string yEvent);
        Task<CurrentQuestionDto> SetCurrentQuestionStatus(string yEvent, int questionId, int status);
        Task<string> UpdateFundAmountAsync(string yEvent, int teamNum, decimal? dollarAmount);
        Task<string> ResetEvent(string yEvent);
        Task<string> SetRound3Answer(List<Round3AnswerDto> round3Answers);
        Task<List<Round3QuestionDto>> GetRound3Master(string yEvent);
        Task<List<IntroDto>> GetRound3Teams(string yEvent);
    }
}
