using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    [Route("api/round2")]
    public class Round2Controller : ControllerBase
    {

        private readonly ILogger<Round2Controller> _logger;
        private readonly IScoreService _scoreService;
        private readonly IManageEventService _manageEventService;
        private readonly IHubContext<EventHub> _eventHub;

        public Round2Controller(ILogger<Round2Controller> logger, IScoreService scoreService, IManageEventService manageEventService,
                                IHubContext<EventHub> eventHub)
        {
            _logger = logger;
            _scoreService = scoreService;
            _manageEventService = manageEventService;
            _eventHub = eventHub;
        }

        [Authorize(Roles = "admin")]
        [HttpGet("allSurvey/{yEvent}")]
        [SwaggerOperation(Summary = "Get all of the survey questions and answers for use of the operators.")]
        public async Task<ActionResult<List<Round2SurveyList>>> GetRound2SurveyMasterAsync(string yEvent)
            => Ok(await _manageEventService.GetRound2SurveyMaster(yEvent));

        [Authorize(Roles = "admin")]
        [HttpGet("allQuestions/{yEvent}")]
        [SwaggerOperation(Summary = "Get all of the survey questions for use of the host.")]
        public async Task<ActionResult<List<Round2SurveyList>>> GetRound2QuestionListAsync(string yEvent)
            => Ok(await _manageEventService.GetRound2QuestionList(yEvent));

        [Authorize(Roles = "admin")]
        [HttpGet("bigBoard/{yEvent}/{teamNo}")]
        [SwaggerOperation(Summary = "Returns the big board for round 2")]
        public async Task<ActionResult<Round2BoardDto>> GetRound2DisplayBoardAsync(string yEvent, int teamNo)
            => Ok(await _scoreService.GetRound2DisplayBoard(yEvent, teamNo));

        [Authorize(Roles = "admin")]
        [HttpPost("teamanswer/text")]
        [SwaggerOperation(Summary = "Saves the team answer with points")]
        public async Task<ActionResult<string>> SetRound2AnswerTextAsync(Round2AnswerDto submitAnswer)
        {
            var returnVar = await _manageEventService.SetRound2AnswerText(submitAnswer);
            await _eventHub.Clients.All.SendAsync("round2Answer");
            return Ok(returnVar);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("teamanswer/survey")]
        [SwaggerOperation(Summary = "Saves the team answer from a direct match to the survey")]
        public async Task<ActionResult<string>> SetRound2AnswerSurveyAsync(Round2AnswerDto submitAnswer)
        {
            var returnVar = await _manageEventService.SetRound2AnswerSurvey(submitAnswer);
            await _eventHub.Clients.All.SendAsync("round2Answer");
            return Ok(returnVar);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("scoreboard/{yEvent}")]
        [SwaggerOperation(Summary = "Returns the scoreboard for round 2")]
        public async Task<ActionResult<Round23Scores>> GetRound23ScoresAsync(string yEvent)
            => Ok(await _scoreService.GetRound23Scores(yEvent, 2, 6));

        [Authorize(Roles = "admin")]
        [HttpGet("bigboard/reveal/{entryNum}")]
        [SwaggerOperation(Summary = "Send message to reveal big board answer.")]
        public async Task<ActionResult> RevealAnswerAsync(int entryNum)
        {
            await _eventHub.Clients.All.SendAsync("round2AnswerShow", entryNum);
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpGet("bigboard/playerone")]
        [SwaggerOperation(Summary = "Send message to reveal all Player 1 answers on the big board.")]
        public async Task<ActionResult> RevealPlayerOneAsync()
        {
            await _eventHub.Clients.All.SendAsync("round2ShowPlayer1");
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpGet("bigboard/changeTeam/{teamNum}")]
        [SwaggerOperation(Summary = "Change team for the big board.")]
        public async Task<ActionResult> ChangeDisplayBoardTeamAsync(int teamNum)
        {
            await _eventHub.Clients.All.SendAsync("round2ChangeTeam", teamNum, 6);
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpGet("updateScoreboard")]
        [SwaggerOperation(Summary = "Sends message to update the scoreboard.")]
        public async Task<ActionResult> UpdateScoreboardAsync()
        {
            // add in controller here
            await _eventHub.Clients.All.SendAsync("round2ScoreUpdate");
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("finalize/{yEvent}")]
        [SwaggerOperation(Summary = "Finalize round 2")]
        public async Task<ActionResult<string>> FinalizeRoundAsync(string yEvent)
            => Ok(await _manageEventService.FinalizeRound(yEvent, 2));

        [Authorize(Roles = "admin")]
        [HttpGet("firstPlayersAnswers/{yEvent}/{teamNum}")]
        [SwaggerOperation(Summary = "Returns the first Players answers for round 2")]
        public async Task<ActionResult<Round23Scores>> GetFirstPlayersAnswersAsync(string yEvent, int teamNum)
            => Ok(await _scoreService.GetFirstPlayersAnswers(yEvent, teamNum));

        // Countdown SignalR.
        [Authorize(Roles = "admin")]
        [HttpGet("countdown/start/{seconds}")]
        [SwaggerOperation(Summary = "Start countdown.")]
        public async Task<ActionResult> StartCountdownAsync(int seconds) {
            await _eventHub.Clients.All.SendAsync("startCountdown", seconds);
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpGet("countdown/stop")]
        [SwaggerOperation(Summary = "Stops countdown.")]
        public async Task<ActionResult> StopCountdownAsync() {
            await _eventHub.Clients.All.SendAsync("stopCountdown");
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpGet("countdown/set/{seconds}")]
        [SwaggerOperation(Summary = "Sets the countdown.")]
        public async Task<ActionResult> SetCountdownAsync(int seconds) {
            await _eventHub.Clients.All.SendAsync("setCountdown", seconds);
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("changePage/{page}")]
        [SwaggerOperation(Summary = "Sends message to change the page.")]
        public async Task<ActionResult> ChangeIntroPageAsync(string page)
        {
            // add in controller here
            await _eventHub.Clients.All.SendAsync("round2ChangePage", page);
            return Ok();
        }

    }
}
