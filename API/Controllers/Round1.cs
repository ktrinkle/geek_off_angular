using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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

        [Authorize(Role.admin)]
        [HttpGet("bigDisplay/{yEvent}")]
        [SwaggerOperation(Summary = "Get a single round 1 question for the big display with media.")]
        public async Task<ActionResult<List<Round1QuestionDisplay>>> GetRound1QuestionAsync(string yEvent)
            => Ok(await _questionService.GetRound1QuestionAsync(yEvent));

        [Authorize(Role.admin)]
        [HttpGet("getAnswers/{yEvent}/{questionId}")]
        [SwaggerOperation(Summary = "Get a single round 1 question and answer for the contestants.")]
        public async Task<ActionResult<Round1QuestionDto>> GetRound1AnswersAsync(string yEvent, int questionId)
            => Ok(await _questionService.GetRound1QuestionWithAnswer(yEvent, questionId));

        [Authorize(Role.player)]
        [HttpGet("getAnswerList/{yEvent}")]
        [SwaggerOperation(Summary = "Get a list of round 1 question and answers for the contestants.")]
        public async Task<ActionResult<List<Round1QuestionDto>>> GetRound1AnswerListAsync(string yEvent)
            => Ok(await _questionService.GetRound1QuestionListWithAnswers(yEvent));

        [Authorize(Role.admin)]
        [HttpGet("getAllQuestions/{yEvent}")]
        [SwaggerOperation(Summary = "Get all of the survey questions and answers for use of the operators.")]
        public async Task<ActionResult<List<Round1QuestionControlDto>>> GetAllRound1QuestionsAsync(string yEvent)
            => Ok(await _questionService.GetAllRound1Questions(yEvent));

        [Authorize]
        [HttpPut("submitAnswer")]
        [SwaggerOperation(Summary = "Player submits the answer to the controlling system")]
        public async Task<ActionResult<string>> SubmitRound1AnswerAsync(Round1EnteredAnswers answers)
        {
            var test = int.TryParse(User.TeamId(), out var answerTeam);
            if (!test)
            {
                answerTeam = 0;
            }

            var submitAnswer = await _questionService.SubmitRound1Answer(answers.Yevent, answers.QuestionNum, answers.TextAnswer, answerTeam);
            await _eventHub.Clients.All.SendAsync("round1PlayerAnswer");
            return Ok(submitAnswer ? "Your answer is in. Good luck!" : "We had a problem. Please try again.");
        }

        [Authorize(Role.admin)]
        [HttpGet("updateAnswerState/{questionNum}/{status}")]
        [SwaggerOperation(Summary = "Show answer choices to contestants and on big board.")]
        public async Task<ActionResult> ShowAnswersToEventAsync()
        {
            await _eventHub.Clients.All.SendAsync("round1ShowAnswerChoices");
            return Ok();
        }

        [Authorize(Role.admin)]
        [HttpGet("openAnswer")]
        [SwaggerOperation(Summary = "Open contestants for answers.")]
        public async Task<ActionResult> OpenAnswerToContestantAsync()
        {
            await _eventHub.Clients.All.SendAsync("round1OpenAnswer");
            return Ok();
        }

        [Authorize(Role.admin)]
        [HttpGet("closeAnswer")]
        [SwaggerOperation(Summary = "Close answers to contestants.")]
        public async Task<ActionResult> CloseAnswerToContestantAsync()
        {
            await _eventHub.Clients.All.SendAsync("round1CloseAnswer");
            return Ok();
        }

        [Authorize(Role.admin)]
        [HttpGet("showTeamAnswer/{yEvent}/{questionId}")]
        [SwaggerOperation(Summary = "Show entered answers")]
        public async Task<ActionResult<List<Round1EnteredAnswers>>> ShowRound1TeamEnteredAnswersAsync(string yEvent, int questionId)
        {
            var returnDto = await _manageEventService.ShowRound1TeamEnteredAnswers(yEvent, questionId);
            // add in controller here
            return Ok(returnDto);
        }

        [Authorize(Role.admin)]
        [HttpPut("scoreAnswer/{yEvent}/{questionId}")]
        [SwaggerOperation(Summary = "Scores the answer automatically")]
        public async Task<ActionResult<string>> ScoreAnswerAutomaticAsync(string yEvent, int questionId)
        {
            var returnString = await _scoreService.ScoreAnswerAutomatic(yEvent, questionId);
            await _eventHub.Clients.All.SendAsync("round1ScoreUpdate");
            return Ok(returnString);
        }

        [Authorize(Role.admin)]
        [HttpPut("scoreManualAnswer/{yEvent}/{questionId}/{teamNum}")]
        [SwaggerOperation(Summary = "Scores the answer manually based on team")]
        public async Task<ActionResult> ScoreAnswerManualAsync(string yEvent, int questionId, int teamNum)
            => Ok(await _scoreService.ScoreAnswerManual(yEvent, questionId, teamNum));

        [Authorize(Role.admin)]
        [HttpPut("updateScoreboard")]
        [SwaggerOperation(Summary = "Sends message to update the scoreboard.")]
        public async Task<ActionResult> UpdateScoreboardAsync()
        {
            // add in controller here
            await _eventHub.Clients.All.SendAsync("round1ScoreUpdate");
            return Ok();
        }

        [Authorize(Role.admin)]
        [HttpPut("changeIntroPage/{introPage}")]
        [SwaggerOperation(Summary = "Sends message to change the intro page.")]
        public async Task<ActionResult> ChangeIntroPageAsync(string introPage)
        {
            // add in controller here
            await _eventHub.Clients.All.SendAsync("round1intro", introPage);
            return Ok();
        }

        [Authorize(Role.admin)]
        [HttpPut("moveQuestion/{questionId}")]
        [SwaggerOperation(Summary = "Sends message to update the question display. This message gets sent to change slides, so to speak. It won't show the answers.")]
        public async Task<ActionResult> ChangeQuestionAsync(int questionId)
        {
            await _eventHub.Clients.All.SendAsync("round1question", questionId);
            return Ok();
        }

        [Authorize(Role.admin)]
        [HttpPut("animateText")]
        [SwaggerOperation(Summary = "Sends message to animate intro screen text.")]
        public async Task<ActionResult> ShowMediaAsync()
        {
            await _eventHub.Clients.All.SendAsync("round1Animate");
            return Ok();
        }

        [Authorize(Role.admin)]
        [HttpPut("animateSeatbelt")]
        [SwaggerOperation(Summary = "Sends message to animate intro screen text.")]
        public async Task<ActionResult> ChangeIntroSeatBeltAsync()
        {
            // add in controller here
            await _eventHub.Clients.All.SendAsync("introSeatbelt");
            return Ok();
        }

        [Authorize(Role.admin)]
        [HttpPut("showMedia/{questionId}")]
        [SwaggerOperation(Summary = "Sends message to show any media on the round 1 big board.")]
        public async Task<ActionResult> ShowMediaAsync(int questionId)
        {
            // add in controller here
            await _eventHub.Clients.All.SendAsync("round1ShowMedia", questionId);
            return Ok();
        }

        [Authorize(Role.admin)]
        [HttpPut("updateStatus/{yEvent}/{questionId}/{status}")]
        [SwaggerOperation(Summary = "Updates status of question and sends message to display. Changes the state for the contestant and big screen.")]
        public async Task<ActionResult<CurrentQuestionDto>> ChangeQuestionAsync(string yEvent, int questionId, int status)
        {
            var returnDto = await _manageEventService.SetCurrentQuestionStatus(yEvent, questionId, status);

            var payload = new CurrentQuestionDto()
            {
                QuestionNum = questionId,
                Status = status
            };
            await _eventHub.Clients.All.SendAsync("round1UpdateContestant", payload);
            return Ok(returnDto);
        }

        [Authorize(Role.admin)]
        [HttpGet("scoreboard/{yEvent}")]
        [SwaggerOperation(Summary = "Gets the round 1 scoreboard")]
        public async Task<ActionResult<List<Round1Scores>>> GetScoreboardAsync(string yEvent)
            => Ok(await _scoreService.GetRound1Scores(yEvent));


        [Authorize(Role.admin)]
        [HttpGet("teamList/{yEvent}")]
        [SwaggerOperation(Summary = "Gets a list of all team members.")]
        public async Task<ActionResult<List<IntroDto>>> GetTeamListAsync(string yEvent) =>
            // HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            await _manageEventService.GetTeamList(yEvent);

        [Authorize(Role.admin)]
        [HttpPut("finalize/{yEvent}")]
        [SwaggerOperation(Summary = "Finalize round 1")]
        public async Task<ActionResult<string>> FinalizeRoundAsync(string yEvent)
            => Ok(await _manageEventService.FinalizeRound(yEvent, 1));
    }
}
