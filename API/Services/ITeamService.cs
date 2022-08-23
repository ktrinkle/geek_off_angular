using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GeekOff.Models;

namespace GeekOff.Services
{
    public interface ITeamService
    {
        public Task<NewTeamEntry> AddNewEventTeamAsync(string yEvent, string? teamName);
        public Task<ApiResponse> MoveTeamNumberAsync(string yEvent, int teamNum, int newTeamNum);
    }
}
