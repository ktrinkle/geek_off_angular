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
        Task<string> SetRound2AnswerText(string yEvent, int questionNum, int teamNum, int playerNum, string answer, int score);
        Task<string> SetRound2AnswerSurvey(string yEvent, int questionNum, int teamNum, int playerNum, int answerNum);
        Task<string> FinalizeRound(string yEvent);
    }
}