using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GeekOff.Data;
using GeekOff.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GeekOff.Services
{
    public class TeamService : ITeamService
    {
        private readonly ContextGo _contextGo;
        private readonly ILogger<TeamService> _logger;
        public TeamService(ILogger<TeamService> logger, ContextGo context)
        {
            _logger = logger;
            _contextGo = context;
        }

        // shell for the future

        public async Task<NewTeamEntry> AddNewEventTeamAsync(string yEvent, string? teamName)
        {
            // check the DB to see highest team number. Note, we don't reuse team numbers.
            var maxTeamNum = await _contextGo.Teamreference.Where(tr=>tr.Yevent == yEvent).MaxAsync(tr => tr.TeamNum);

            maxTeamNum += 1;

            var newTeamDb = new Teamreference() {
                Yevent = yEvent,
                TeamNum = maxTeamNum,
                Teamname = teamName,
                TeamGuid = new Guid()
            };

            await _contextGo.AddAsync(newTeamDb);
            await _contextGo.SaveChangesAsync();

            return new NewTeamEntry()
            {
                TeamNum = newTeamDb.TeamNum,
                TeamGuid = newTeamDb.TeamGuid,
                SuccessInd = true
            };

        }

        public async Task<ApiResponse> MoveTeamNumberAsync(string yEvent, int teamNum, int newTeamNum)
        {
            var response = new ApiResponse();

            return response;
        }

    }
}
