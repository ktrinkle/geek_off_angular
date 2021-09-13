using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeekOff.Models;

namespace GeekOff.Services
{
    public interface IManageEventService
    {
        Task<List<Round2SurveyList>> GetRound2SurveyMaster(string yEvent);
        Task<string> SetRound2Answer(string yEvent, int questionNo, int teamNo, int playerNum, string answer, int score);
        Task<string> FinalizeRound(string yEvent);
    }
}