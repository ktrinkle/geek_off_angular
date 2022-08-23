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
            var existTeam = await _contextGo.Teamreference.FirstOrDefaultAsync(tr => tr.TeamNum == teamNum && tr.Yevent == yEvent);

            if (existTeam is null)
            {
                return new ApiResponse()
                {
                    SuccessInd = false,
                    Response = "The original team number does not exist."
                };
            }

            var newTeamFlag = await _contextGo.Teamreference.AnyAsync(tr => tr.TeamNum == newTeamNum && tr.Yevent == yEvent);

            if (newTeamFlag)
            {
                return new ApiResponse()
                {
                    SuccessInd = false,
                    Response = "The new team number already exists."
                };
            }

            existTeam.TeamNum = newTeamNum;

            var teamUsers = await _contextGo.TeamUser.Where(tu => tu.TeamNum == teamNum && tu.Yevent == yEvent).ToListAsync();
            foreach(var teamUser in teamUsers)
            {
                teamUser.TeamNum = newTeamNum;
            }

            var scores = await _contextGo.Scoring.Where(s => s.TeamNum == teamNum && s.Yevent == yEvent).ToListAsync();
            foreach(var score in scores)
            {
                score.TeamNum = newTeamNum;
            }

            var userAnswers = await _contextGo.UserAnswer.Where(u => u.TeamNum == teamNum && u.Yevent == yEvent).ToListAsync();
            foreach(var userAnswer in userAnswers)
            {
                userAnswer.TeamNum = newTeamNum;
            }

            var roundResults = await _contextGo.Roundresult.Where(u => u.TeamNum == teamNum && u.Yevent == yEvent).ToListAsync();
            foreach(var roundResult in roundResults)
            {
                roundResult.TeamNum = newTeamNum;
            }

            _contextGo.Update(existTeam);
            _contextGo.UpdateRange(teamUsers);
            _contextGo.UpdateRange(scores);
            _contextGo.UpdateRange(userAnswers);
            _contextGo.UpdateRange(roundResults);
            await _contextGo.SaveChangesAsync();

            return new ApiResponse()
            {
                SuccessInd = true,
                Response = "The team was successfully moved to the new number."
            };
        }

    }
}
