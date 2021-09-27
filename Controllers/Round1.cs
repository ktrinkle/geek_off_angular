using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.SignalR;
using GeekOff.Services;
using GeekOff.Models;

namespace GeekOff.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/round1")]
    public class Round1Controller : ControllerBase
    {

        private readonly ILogger<Round1Controller> _logger;
        private readonly IScoreService _scoreService;
        private readonly IManageEventService _manageEventService;
        private readonly IHubContext<EventHub> _eventHub;

        public Round1Controller(ILogger<Round1Controller> logger, IScoreService scoreService, IManageEventService manageEventService,
                                IHubContext<EventHub> eventHub)
        {
            _logger = logger;
            _scoreService = scoreService;
            _manageEventService = manageEventService;
            _eventHub = eventHub;
        }

        [Authorize(Roles = "Player")]
        [HttpGet("getQuestion/{yEvent}/{questionId}")]
        [SwaggerOperation(Summary = "Get all of the survey questions and answers for use of the operators.")]
        public async Task<ActionResult<string>> GetRound1Question(string yEvent, int questionId)
            => Ok(await _manageEventService.GetRound2SurveyMaster(yEvent));

        [Authorize(Roles = "Player")]
        [HttpGet("getAnswers/{yEvent}/{questionId}")]
        [SwaggerOperation(Summary = "Get all of the survey questions and answers for use of the operators.")]
        public async Task<ActionResult<List<Round1Answers>>> GetRound1Answers(string yEvent, int questionId)
            => Ok(await _manageEventService.GetRound2SurveyMaster(yEvent));

        [Authorize(Roles = "Player")]
        [HttpPut("submitAnswer/{yEvent}/{questionId}/{answerText}")]
        [SwaggerOperation(Summary = "Player submits the answer to the controlling system")]
        public async Task<ActionResult<string>> SubmitRound1Answer(string yEvent, int questionId, string answerText)
        {
            var submitAnswer = true;
            await _eventHub.Clients.All.SendAsync("round1PlayerAnswer");
            return Ok(submitAnswer ? "Your answer is in. Good luck!" : "We had a problem. Please try again.");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("showAnswerChoices")]
        [SwaggerOperation(Summary = "Show answer choices to contestants and on big board.")]
        public async Task<ActionResult> ShowAnswersToEvent()
        {
            // add in controller here
            await _eventHub.Clients.All.SendAsync("round1ShowAnswerChoices");
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("openAnswer")]
        [SwaggerOperation(Summary = "Open contestants for answers.")]
        public async Task<ActionResult> OpenAnswerToContestant()
        {
            // add in controller here
            await _eventHub.Clients.All.SendAsync("round1OpenAnswer");
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("closeAnswer")]
        [SwaggerOperation(Summary = "Close answers to contestants.")]
        public async Task<ActionResult> CloseAnswerToContestant()
        {
            // add in controller here
            await _eventHub.Clients.All.SendAsync("round1CloseAnswer");
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("scoreAnswer/{yEvent}/{questionId}")]
        [SwaggerOperation(Summary = "Show entered answers")]
        public async Task<ActionResult<List<Round1EnteredAnswers>>> ShowRound1TeamEnteredAnswers(string yEvent, int questionId)
        {
            var returnDto = await _manageEventService.ShowRound1TeamEnteredAnswers(yEvent, questionId);
            // add in controller here
            return Ok(returnDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("scoreAnswer/{yEvent}/{questionId}")]
        [SwaggerOperation(Summary = "Scores the answer automatically")]
        public async Task<ActionResult> ScoreAnswerAutomatic(string yEvent, int questionId)
        {
            // add in controller here
            await _eventHub.Clients.All.SendAsync("round1ScoreUpdate");
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("scoreAnswer/{yEvent}/{questionId}/{teamNum}")]
        [SwaggerOperation(Summary = "Scores the answer manually based on team")]
        public async Task<ActionResult> ScoreAnswerManual(string yEvent, int questionId, int teamNum)
        {
            // add in controller here
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("updateScoreboard")]
        [SwaggerOperation(Summary = "Sends message to update the scoreboard.")]
        public async Task<ActionResult> UpdateScoreboard()
        {
            // add in controller here
            await _eventHub.Clients.All.SendAsync("round1ScoreUpdate");
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("scoreboard/{yEvent}")]
        [SwaggerOperation(Summary = "Gets the round 1 scoreboard")]
        public async Task<ActionResult<List<Round1Scores>>> GetScoreboard(string yEvent)
        {
            // add in controller here
            return Ok();
        }
    }
}