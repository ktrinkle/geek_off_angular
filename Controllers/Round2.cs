using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using GeekOff.Services;
using GeekOff.Models;

namespace GeekOff.Controllers
{
    [ApiController]
    // [Authorize]
    [Route("api/round2")]
    public class Round2Controller : ControllerBase
    {

        private readonly ILogger<Round2Controller> _logger;
        private readonly IScoreService _scoreService;
        private readonly IManageEventService _manageEventService;

        public Round2Controller(ILogger<Round2Controller> logger, IScoreService scoreService, IManageEventService manageEventService)
        {
            _logger = logger;
            _scoreService = scoreService;
            _manageEventService = manageEventService;
        }

        [HttpGet("allSurvey/{yEvent}")]
        [SwaggerOperation(Summary = "Get all of the survey questions for use of the operators.")]
        public async Task<ActionResult<List<Round2SurveyList>>> GetRound2SurveyMaster(string yEvent)
            => Ok(await _manageEventService.GetRound2SurveyMaster(yEvent));

        [HttpGet("bigBoard")]
        [SwaggerOperation(Summary = "Returns the big board for round 2")]
        public async Task<ActionResult<Round2BoardDto>> GetRound2Scoreboard()
            => Ok(await _scoreService.GetRound2Scoreboard());

        [HttpPost("teamAnswer")]
        [SwaggerOperation(Summary = "Saves the team answer with points")]
        public async Task<ActionResult<string>> SetRound2Answer(string yEvent, int questionNo, int teamNo, int playerNum, string answer, int score)
            => Ok(await _manageEventService.SetRound2Answer(yEvent, questionNo, teamNo, playerNum, answer, score));

        [HttpPut("finalize")]
        [SwaggerOperation(Summary = "Finalize round 2")]
        public async Task<ActionResult<string>> FinalizeRound(string yEvent)
            => Ok(await _manageEventService.FinalizeRound(yEvent));
        
    }
}