using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Microsoft.Identity.Web.Resource;
using GeekOff.Services;
using GeekOff.Models;

namespace GeekOff.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/round1")]
    public class Round1Controller : ControllerBase
    {
        static readonly string[] scopeRequiredByApi = new string[] { "access_as_user" };
        private readonly ILogger<Round1Controller> _logger;
        private readonly IScoreService _scoreService;
        private readonly IManageEventService _manageEventService;
        private readonly IHubContext<EventHub> _eventHub;
        private readonly IQuestionService _questionService;

        public Round1Controller(ILogger<Round1Controller> logger, IScoreService scoreService, IManageEventService manageEventService,
                                IHubContext<EventHub> eventHub, IQuestionService questionService)
        {
            _logger = logger;
            _scoreService = scoreService;
            _manageEventService = manageEventService;
            _eventHub = eventHub;
            _questionService = questionService;
        }

        [Authorize(Roles = "admin,player")]
        [HttpGet("getQuestion/{yEvent}/{questionId}")]
        [SwaggerOperation(Summary = "Get all of the survey questions and answers for use of the operators.")]
        public async Task<ActionResult<Round1QuestionDto>> GetRound1Question(string yEvent, int questionId)
            => Ok(await _questionService.GetRound1Question(yEvent, questionId));

        [Authorize(Roles = "admin,player")]
        [HttpGet("getAnswers/{yEvent}/{questionId}")]
        [SwaggerOperation(Summary = "Get all of the survey questions and answers for use of the operators.")]
        public async Task<ActionResult<Round1QuestionDto>> GetRound1Answers(string yEvent, int questionId)
            => Ok(await _questionService.GetRound1QuestionWithAnswer(yEvent, questionId));

        [Authorize(Roles = "admin,player")]
        [HttpPut("submitAnswer/{yEvent}/{questionId}/{answerText}")]
        [SwaggerOperation(Summary = "Player submits the answer to the controlling system")]
        public async Task<ActionResult<string>> SubmitRound1Answer(string yEvent, int questionId, string answerText)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            int teamNo = 0;
            string answerUser = "362525";
            
            var submitAnswer = await _questionService.SubmitRound1Answer(yEvent, questionId, teamNo, answerText, answerUser);
            await _eventHub.Clients.All.SendAsync("round1PlayerAnswer");
            return Ok(submitAnswer ? "Your answer is in. Good luck!" : "We had a problem. Please try again.");
        }

        [Authorize(Roles = "admin")]
        [HttpGet("showAnswerChoices")]
        [SwaggerOperation(Summary = "Show answer choices to contestants and on big board.")]
        public async Task<ActionResult> ShowAnswersToEvent()
        {
            // add in controller here
            await _eventHub.Clients.All.SendAsync("round1ShowAnswerChoices");
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpGet("openAnswer")]
        [SwaggerOperation(Summary = "Open contestants for answers.")]
        public async Task<ActionResult> OpenAnswerToContestant()
        {
            // add in controller here
            await _eventHub.Clients.All.SendAsync("round1OpenAnswer");
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpGet("closeAnswer")]
        [SwaggerOperation(Summary = "Close answers to contestants.")]
        public async Task<ActionResult> CloseAnswerToContestant()
        {
            // add in controller here
            await _eventHub.Clients.All.SendAsync("round1CloseAnswer");
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("showTeamAnswer/{yEvent}/{questionId}")]
        [SwaggerOperation(Summary = "Show entered answers")]
        public async Task<ActionResult<List<Round1EnteredAnswers>>> ShowRound1TeamEnteredAnswers(string yEvent, int questionId)
        {
            var returnDto = await _manageEventService.ShowRound1TeamEnteredAnswers(yEvent, questionId);
            // add in controller here
            return Ok(returnDto);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("scoreAnswer/{yEvent}/{questionId}")]
        [SwaggerOperation(Summary = "Scores the answer automatically")]
        public async Task<ActionResult> ScoreAnswerAutomatic(string yEvent, int questionId)
        {
            // add in controller here
            await _eventHub.Clients.All.SendAsync("round1ScoreUpdate");
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("scoreAnswer/{yEvent}/{questionId}/{teamNum}")]
        [SwaggerOperation(Summary = "Scores the answer manually based on team")]
        public async Task<ActionResult> ScoreAnswerManual(string yEvent, int questionId, int teamNum)
        {
            // add in controller here
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("updateScoreboard")]
        [SwaggerOperation(Summary = "Sends message to update the scoreboard.")]
        public async Task<ActionResult> UpdateScoreboard()
        {
            // add in controller here
            await _eventHub.Clients.All.SendAsync("round1ScoreUpdate");
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("changeIntroPage/{introPage}")]
        [SwaggerOperation(Summary = "Sends message to change the intro page.")]
        public async Task<ActionResult> ChangeIntroPage(string introPage)
        {
            // add in controller here
            await _eventHub.Clients.All.SendAsync("round1intro", introPage);
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("moveQuestion/{questionId}")]
        [SwaggerOperation(Summary = "Sends message to update the question display. This message gets sent to change slides, so to speak. It won't show the answers.")]
        public async Task<ActionResult> ChangeQuestion(int questionId)
        {
            // add in controller here
            await _eventHub.Clients.All.SendAsync("round1question", questionId);
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("updateStatus/{yEvent}/{questionId}/{status}")]
        [SwaggerOperation(Summary = "Updates status of question and sends message to display. Changes the state for the contestant and big screen.")]
        public async Task<ActionResult<CurrentQuestionDto>> ChangeQuestion(string yEvent, int questionId, int status)
        {
            var returnDto = await _manageEventService.SetCurrentQuestionStatus(yEvent, questionId, status);

            var messageToSend = "";
            // @TODO: refactor this as a switch when i can remember the syntax
            if (status == 0)
            {
                messageToSend = "round1Question";
            }
            if (status == 1)
            {
                messageToSend = "round1ShowAnswerChoices";
            }
            if (status == 2)
            {
                messageToSend = "round1OpenAnswer";
            }
            if (status == 3)
            {
                messageToSend = "round1CloseAnswer";
            }

            await _eventHub.Clients.All.SendAsync(messageToSend, questionId);
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpGet("scoreboard/{yEvent}")]
        [SwaggerOperation(Summary = "Gets the round 1 scoreboard")]
        public async Task<ActionResult<List<Round1Scores>>> GetScoreboard(string yEvent)
        {
            // add in controller here
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpGet("teamList/{yEvent}")]
        [SwaggerOperation(Summary = "Gets a list of all team members.")]
        public async Task<ActionResult<List<IntroDto>>> GetTeamList(string yEvent)
        {
            // HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            return await _manageEventService.GetTeamList(yEvent);
        }
    }
}