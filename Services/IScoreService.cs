using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeekOff.Models;

namespace GeekOff.Services
{
    public interface IScoreService
    {
        Task<Round2BoardDto> GetRound2Scoreboard();
    }
}