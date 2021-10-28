using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GeekOff.Helpers;
using GeekOff.Models;
using GeekOff.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using Swashbuckle.AspNetCore.Annotations;

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

        [Authorize(Roles = "admin")]
        [HttpGet("getQuestion/{yEvent}/{questionId}")]
        [SwaggerOperation(Summary = "Get a single round 1 question for the big display with media.")]
        public async Task<ActionResult<Round1QuestionDisplay>> GetRound1QuestionAsync(string yEvent, int questionId)
            => Ok(await _questionService.GetRound1Question(yEvent, questionId));

        [Authorize(Roles = "admin,player")]
        [HttpGet("getAnswers/{yEvent}/{questionId}")]
        [SwaggerOperation(Summary = "Get a single round 1 question and answer for the contestants.")]
        public async Task<ActionResult<Round1QuestionDto>> GetRound1AnswersAsync(string yEvent, int questionId)
            => Ok(await _questionService.GetRound1QuestionWithAnswer(yEvent, questionId));

        [Authorize(Roles = "admin,player")]
        [HttpGet("getAnswerList/{yEvent}")]
        [SwaggerOperation(Summary = "Get a single round 1 question and answer for the contestants.")]
        public async Task<ActionResult<List<Round1QuestionDto>>> GetRound1AnswerListAsync(string yEvent)
            => Ok(await _questionService.GetRound1QuestionListWithAnswers(yEvent));

        [Authorize(Roles = "admin")]
        [HttpGet("getAllQuestions/{yEvent}")]
        [SwaggerOperation(Summary = "Get all of the survey questions and answers for use of the operators.")]
        public async Task<ActionResult<List<Round1QuestionControlDto>>> GetAllRound1QuestionsAsync(string yEvent)
            => Ok(await _questionService.GetAllRound1Questions(yEvent));

        [Authorize(Roles = "admin,player")]
        [HttpPut("submitAnswer")]
        [SwaggerOperation(Summary = "Player submits the answer to the controlling system")]
        public async Task<ActionResult<string>> SubmitRound1AnswerAsync(Round1EnteredAnswers answers)
        {
            var answerUser = User.UserId();

            var submitAnswer = await _questionService.SubmitRound1Answer(answers.Yevent, answers.QuestionNum, answers.TextAnswer, answerUser);
            await _eventHub.Clients.All.SendAsync("round1PlayerAnswer");
            return Ok(submitAnswer ? "Your answer is in. Good luck!" : "We had a problem. Please try again.");
        }

        [Authorize(Roles = "admin")]
        [HttpGet("showAnswerChoices")]
        [SwaggerOperation(Summary = "Show answer choices to contestants and on big board.")]
        public async Task<ActionResult> ShowAnswersToEventAsync()
        {
            await _eventHub.Clients.All.SendAsync("round1ShowAnswerChoices");
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpGet("openAnswer")]
        [SwaggerOperation(Summary = "Open contestants for answers.")]
        public async Task<ActionResult> OpenAnswerToContestantAsync()
        {
            await _eventHub.Clients.All.SendAsync("round1OpenAnswer");
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpGet("closeAnswer")]
        [SwaggerOperation(Summary = "Close answers to contestants.")]
        public async Task<ActionResult> CloseAnswerToContestantAsync()
        {
            await _eventHub.Clients.All.SendAsync("round1CloseAnswer");
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpGet("showTeamAnswer/{yEvent}/{questionId}")]
        [SwaggerOperation(Summary = "Show entered answers")]
        public async Task<ActionResult<List<Round1EnteredAnswers>>> ShowRound1TeamEnteredAnswersAsync(string yEvent, int questionId)
        {
            var returnDto = await _manageEventService.ShowRound1TeamEnteredAnswers(yEvent, questionId);
            // add in controller here
            return Ok(returnDto);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("scoreAnswer/{yEvent}/{questionId}")]
        [SwaggerOperation(Summary = "Scores the answer automatically")]
        public async Task<ActionResult<string>> ScoreAnswerAutomaticAsync(string yEvent, int questionId)
        {
            var returnString = await _scoreService.ScoreAnswerAutomatic(yEvent, questionId);
            await _eventHub.Clients.All.SendAsync("round1ScoreUpdate");
            return Ok(returnString);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("scoreManualAnswer/{yEvent}/{questionId}/{teamNum}")]
        [SwaggerOperation(Summary = "Scores the answer manually based on team")]
        public async Task<ActionResult> ScoreAnswerManualAsync(string yEvent, int questionId, int teamNum)
            => Ok(await _scoreService.ScoreAnswerManual(yEvent, questionId, teamNum));

        [Authorize(Roles = "admin")]
        [HttpPut("updateScoreboard")]
        [SwaggerOperation(Summary = "Sends message to update the scoreboard.")]
        public async Task<ActionResult> UpdateScoreboardAsync()
        {
            // add in controller here
            await _eventHub.Clients.All.SendAsync("round1ScoreUpdate");
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("changeIntroPage/{introPage}")]
        [SwaggerOperation(Summary = "Sends message to change the intro page.")]
        public async Task<ActionResult> ChangeIntroPageAsync(string introPage)
        {
            // add in controller here
            await _eventHub.Clients.All.SendAsync("round1intro", introPage);
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("moveQuestion/{questionId}")]
        [SwaggerOperation(Summary = "Sends message to update the question display. This message gets sent to change slides, so to speak. It won't show the answers.")]
        public async Task<ActionResult> ChangeQuestionAsync(int questionId)
        {
            await _eventHub.Clients.All.SendAsync("round1question", questionId);
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("animateText")]
        [SwaggerOperation(Summary = "Sends message to animate intro screen text.")]
        public async Task<ActionResult> ShowMediaAsync()
        {
            await _eventHub.Clients.All.SendAsync("round1Animate");
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("refreshState/{currentQuestion}/{status}")]
        [SwaggerOperation(Summary = "Sends message to animate intro screen text.")]
        public async Task<ActionResult> RefreshClientStateAsync(int currentQuestion, int status)
        {
            var payload = new CurrentQuestionDto()
            {
                QuestionNum = currentQuestion,
                Status = status
            }
            await _eventHub.Clients.All.SendAsync("round1RefreshContestant", payload);
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("animateSeatbelt")]
        [SwaggerOperation(Summary = "Sends message to animate intro screen text.")]
        public async Task<ActionResult> ChangeIntroSeatBeltAsync()
        {
            // add in controller here
            await _eventHub.Clients.All.SendAsync("introSeatbelt");
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("showMedia/{questionId}")]
        [SwaggerOperation(Summary = "Sends message to show any media on the round 1 big board.")]
        public async Task<ActionResult> ShowMediaAsync(int questionId)
        {
            // add in controller here
            await _eventHub.Clients.All.SendAsync("round1ShowMedia", questionId);
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("updateStatus/{yEvent}/{questionId}/{status}")]
        [SwaggerOperation(Summary = "Updates status of question and sends message to display. Changes the state for the contestant and big screen.")]
        public async Task<ActionResult<CurrentQuestionDto>> ChangeQuestionAsync(string yEvent, int questionId, int status)
        {
            var returnDto = await _manageEventService.SetCurrentQuestionStatus(yEvent, questionId, status);

            var messageToSend = status switch
            {
                0 => "round1Question",
                1 => "round1ShowAnswerChoices",
                2 => "round1OpenAnswer",
                3 => "round1CloseAnswer",
                _ => ""
            };

            await _eventHub.Clients.All.SendAsync(messageToSend, questionId);
            return Ok(returnDto);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("scoreboard/{yEvent}")]
        [SwaggerOperation(Summary = "Gets the round 1 scoreboard")]
        public async Task<ActionResult<List<Round1Scores>>> GetScoreboardAsync(string yEvent)
            => Ok(await _scoreService.GetRound1Scores(yEvent));


        [Authorize(Roles = "admin")]
        [HttpGet("teamList/{yEvent}")]
        [SwaggerOperation(Summary = "Gets a list of all team members.")]
        public async Task<ActionResult<List<IntroDto>>> GetTeamListAsync(string yEvent) =>
            // HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            await _manageEventService.GetTeamList(yEvent);

        [Authorize(Roles = "admin")]
        [HttpPut("finalize/{yEvent}")]
        [SwaggerOperation(Summary = "Finalize round 1")]
        public async Task<ActionResult<string>> FinalizeRoundAsync(string yEvent)
            => Ok(await _manageEventService.FinalizeRound(yEvent, 1));
    }
}
