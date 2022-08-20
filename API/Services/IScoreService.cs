using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeekOff.Models;

namespace GeekOff.Services
{
    public interface IScoreService
    {
        Task<Round2BoardDto> GetRound2DisplayBoard(string yEvent, int TeamNum);
        Task<List<Round1Scores>> GetRound1Scores(string yEvent);
        Task<List<Round23Scores>> GetRound23Scores(string yEvent, int roundNum, int maxRnk);
        Task<string> ScoreAnswerAutomatic(string yEvent, int questionId);
        Task<bool> ScoreAnswerManual(string yEvent, int questionId, int teamNum);
        Task<List<Round2Answers>> GetFirstPlayersAnswers(string yEvent, int teamNum);
    }
}
