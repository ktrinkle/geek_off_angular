using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekOff.Entities;
using GeekOff.Models;
using GeekOff.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace GeekOff.Controllers
{
    [ApiController]
    // [Authorize]
    [Route("api/round3")]
    public class Round3Controller : ControllerBase
    {

        private readonly ILogger<Round3Controller> _logger;
        private readonly IScoreService _scoreService;
        private readonly IManageEventService _manageEventService;
        private readonly IHubContext<EventHub> _eventHub;

        public Round3Controller(ILogger<Round3Controller> logger, IScoreService scoreService, IManageEventService manageEventService,
                                IHubContext<EventHub> eventHub)
        {
            _logger = logger;
            _scoreService = scoreService;
            _manageEventService = manageEventService;
            _eventHub = eventHub;
        }

        [Authorize(Role.admin)]
        [HttpGet("allQuestions/{yEvent}")]
        [SwaggerOperation(Summary = "Get all of the questions and points for use of the operators.")]
        public async Task<ActionResult<List<Round3QuestionDto>>> GetRound3MasterAsync(string yEvent)
            => Ok(await _manageEventService.GetRound3Master(yEvent));

        [Authorize(Role.admin)]
        [HttpGet("allTeams/{yEvent}")]
        [SwaggerOperation(Summary = "Get all of the round 3 teams.")]
        public async Task<ActionResult<List<IntroDto>>> GetRound3TeamsAsync(string yEvent)
            => Ok(await _manageEventService.GetRound3Teams(yEvent));

        [Authorize(Role.admin)]
        [HttpPost("teamanswer")]
        [SwaggerOperation(Summary = "Saves the team answer with points")]
        public async Task<ActionResult<string>> SetRound3AnswerAsync(List<Round3AnswerDto> submitAnswer)
        {
            var returnVar = await _manageEventService.SetRound3Answer(submitAnswer);
            await _eventHub.Clients.All.SendAsync("round3ScoreUpdate");
            return Ok(returnVar);
        }

        [Authorize(Role.admin)]
        [HttpGet("scoreboard/{yEvent}")]
        [SwaggerOperation(Summary = "Returns the scoreboard for round 3")]
        public async Task<ActionResult<Round23Scores>> GetRound23ScoresAsync(string yEvent)
            => Ok(await _scoreService.GetRound23Scores(yEvent, 3, 3));


        [Authorize(Role.admin)]
        [HttpGet("updateScoreboard")]
        [SwaggerOperation(Summary = "Sends message to update the scoreboard.")]
        public async Task<ActionResult> UpdateScoreboardAsync()
        {
            // add in controller here
            await _eventHub.Clients.All.SendAsync("round3ScoreUpdate");
            return Ok();
        }

        [Authorize(Role.admin)]
        [HttpPut("finalize/{yEvent}")]
        [SwaggerOperation(Summary = "Finalize round 3")]
        public async Task<ActionResult<string>> FinalizeRoundAsync(string yEvent)
            => Ok(await _manageEventService.FinalizeRound(yEvent, 3));

    }
}
