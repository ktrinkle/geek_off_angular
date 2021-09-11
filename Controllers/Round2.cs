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
    [Authorize]
    [Route("api/round2")]
    public class Round2Controller : ControllerBase
    {

        private readonly ILogger<Round2Controller> _logger;
        private readonly IScoreService _scoreService;

        public Round2Controller(ILogger<Round2Controller> logger, IScoreService scoreService)
        {
            _logger = logger;
            _scoreService = scoreService;
        }

        [HttpGet("bigBoard")]
        [SwaggerOperation(Summary = "Returns the big board for round 2")]
        public async Task<ActionResult<Round2BoardDto>> GetRound2Scoreboard()
            => Ok(await _scoreService.GetRound2Scoreboard());

        
    }
}