namespace GeekOff.Controllers;

[ApiController]
[Authorize]
[Route("api/round1")]
public class Round1Controller(IHubContext<EventHub> eventHub, IMediator mediator) : ControllerBase
{
    private readonly IHubContext<EventHub> _eventHub = eventHub;
    private readonly IMediator _mediator = mediator;

    [Authorize(Roles = "admin")]
    [HttpGet("bigDisplay/{YEvent}")]
    [SwaggerOperation(Summary = "Get list of the round 1 questions for the big display with media.")]
    public async Task<ActionResult<List<Round1QuestionDisplay>>> GetRound1QuestionsAsync([FromRoute] RoundOneQuestionHandler.Request request)
        => await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            { Status: QueryStatus.NotFound } result => NotFound(result.Value),
            _ => throw new InvalidOperationException()
        };

    [Authorize(Roles = "admin")]
    [HttpGet("getAnswers/{YEvent}/{QuestionId}")]
    [SwaggerOperation(Summary = "Get a single round 1 question and answer for the host.")]
    public async Task<ActionResult<Round1QuestionDto>> GetRound1AnswersAsync([FromRoute] RoundOneSingleQAndAHandler.Request request)
        => await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            { Status: QueryStatus.NotFound } result => NotFound(result.Value),
            _ => throw new InvalidOperationException()
        };

    [Authorize(Roles = "player")]
    [HttpGet("getAnswerList/{YEvent}")]
    [SwaggerOperation(Summary = "Get a list of round 1 question and answers for the contestants.")]
    public async Task<ActionResult<List<Round1QuestionDto>>> GetRound1PlayerQuestionListAsync([FromRoute] RoundOnePlayerQAndAHandler.Request request)
        => await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            { Status: QueryStatus.NotFound } result => NotFound(result.Value),
            _ => throw new InvalidOperationException()
        };

    [Authorize(Roles = "admin")]
    [HttpGet("getAllQuestions/{YEvent}")]
    [SwaggerOperation(Summary = "Get all of the survey questions and answers for use of the operators.")]
    public async Task<ActionResult<List<Round1QuestionControlDto>>> GetAllRound1QuestionsAsync([FromRoute] RoundOneAdminQAndAHandler.Request request)
        => await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            { Status: QueryStatus.NotFound } result => NotFound(result.Value),
            _ => throw new InvalidOperationException()
        };

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

        SubmitAnswerHandler.Request request = new ()
        {
            YEvent = answers.Yevent,
            QuestionNum = answers.QuestionNum,
            TextAnswer = answers.TextAnswer,
            TeamNum = answerTeam,
            RoundNum = 1
        };

        var handlerResult = await _mediator.Send(request);
        await _eventHub.Clients.All.SendAsync("round1PlayerAnswer");

        return handlerResult switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value.Message),
            { Status: QueryStatus.BadRequest } result => BadRequest(result.Value.Message),
            _ => throw new InvalidOperationException()
        };
    }

    [Authorize(Roles = "admin")]
    [HttpGet("showTeamAnswer/{YEvent}/{QuestionNum}")]
    [SwaggerOperation(Summary = "Show entered answers")]
    public async Task<ActionResult<List<Round1EnteredAnswers>>> ShowRound1TeamEnteredAnswersAsync([FromRoute] RoundOneEnteredAnswersHandler.Request request)
        => await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            { Status: QueryStatus.NotFound } result => NotFound(result.Value),
            _ => throw new InvalidOperationException()
        };

    [Authorize(Roles = "admin")]
    [HttpPut("scoreAnswer/{YEvent}/{QuestionNum}")]
    [SwaggerOperation(Summary = "Scores the answer automatically")]
    public async Task<ActionResult<string>> ScoreAnswerAutomaticAsync([FromRoute] ScoreRoundOneAnswerAutomaticHandler.Request request)
    {
        var returnObject = await _mediator.Send(request);
        await _eventHub.Clients.All.SendAsync("round1ScoreUpdate");

        return returnObject switch
        {
            { Status: QueryStatus.Success } result => Ok(returnObject.Value.Message),
            { Status: QueryStatus.NotFound } result => Ok(returnObject.Value.Message),
            _ => throw new InvalidOperationException()
        };
    }

    [Authorize(Roles = "admin")]
    [HttpPut("scoreManualAnswer/{YEvent}/{QuestionNum}/{TeamNum}")]
    [SwaggerOperation(Summary = "Scores the answer manually based on team")]
    public async Task<ActionResult> ScoreAnswerManualAsync([FromRoute] ScoreRoundOneAnswerManualHandler.Request request)
        => await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            { Status: QueryStatus.NotFound } result => NotFound(result.Value),
            { Status: QueryStatus.BadRequest } result => BadRequest(result.Value),
            _ => throw new InvalidOperationException()
        };

    [Authorize(Roles = "admin")]
    [HttpPut("updateStatus/{yEvent}/{questionNum}/{status}")]
    [SwaggerOperation(Summary = "Updates status of question and sends message to display. Changes the state for the contestant and big screen.")]
    public async Task<ActionResult<CurrentQuestionDto>> ChangeQuestionAsync(string yEvent, int questionNum, int status)
    {
        SetCurrentQuestionHandler.Request request = new ()
        {
            YEvent = yEvent,
            QuestionNum = questionNum,
            Status = status,
            RoundNum = 1
        };

        var returnDto = await _mediator.Send(request);

        switch (returnDto.Status)
        {
            case QueryStatus.Success:
                await _eventHub.Clients.All.SendAsync("round1UpdateContestant", returnDto.Value);
                return Ok(returnDto.Value);
            case QueryStatus.NotFound:
                return NotFound(new CurrentQuestionDto());
            default:
                return BadRequest(new CurrentQuestionDto());
        }
    }

    [Authorize(Roles = "admin")]
    [HttpGet("scoreboard/{YEvent}")]
    [SwaggerOperation(Summary = "Gets the round 1 scoreboard")]
    public async Task<ActionResult<List<Round1Scores>>> GetScoreboardAsync([FromRoute] RoundOneScoresHandler.Request request) =>
        await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            { Status: QueryStatus.NotFound } result => NotFound(result.Value),
            _ => throw new InvalidOperationException()
        };

    [Authorize(Roles = "admin")]
    [HttpGet("teamList/{YEvent}")]
    [SwaggerOperation(Summary = "Gets a list of all team members for the intro screen.")]
    public async Task<ActionResult<List<IntroDto>>> GetTeamListAsync([FromRoute] TeamListWithPlayersHandler.Request request) =>
        await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            { Status: QueryStatus.NotFound } result => NotFound(result.Value),
            _ => throw new InvalidOperationException()
        };

    [Authorize(Roles = "admin")]
    [HttpPut("finalize/{yEvent}")]
    [SwaggerOperation(Summary = "Finalize round 1")]
    public async Task<ActionResult<string>> FinalizeRoundAsync(string yEvent)
    {
        FinalizeRoundHandler.Request request = new ()
        {
            YEvent = yEvent,
            RoundNum = 1
        };

        return await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value.Message),
            { Status: QueryStatus.NotFound } result => NotFound(result.Value.Message),
            { Status: QueryStatus.BadRequest } result => BadRequest(result.Value.Message),
            _ => throw new InvalidOperationException()
        };
    }

    #region SignalRcalls

    [Authorize(Roles = "admin")]
    [HttpGet("updateAnswerState")]
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
    [HttpPut("updateScoreboard")]
    [SwaggerOperation(Summary = "Sends message to update the scoreboard.")]
    public async Task<ActionResult> UpdateScoreboardAsync()
    {
        await _eventHub.Clients.All.SendAsync("round1ScoreUpdate");
        return Ok();
    }

    [Authorize(Roles = "admin")]
    [HttpPut("changeIntroPage/{introPage}")]
    [SwaggerOperation(Summary = "Sends message to change the intro page.")]
    public async Task<ActionResult> ChangeIntroPageAsync(string introPage)
    {
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
    [HttpPut("animateSeatbelt")]
    [SwaggerOperation(Summary = "Sends message to animate intro screen text.")]
    public async Task<ActionResult> ChangeIntroSeatBeltAsync()
    {
        await _eventHub.Clients.All.SendAsync("introSeatbelt");
        return Ok();
    }

    [Authorize(Roles = "admin")]
    [HttpPut("showMedia/{questionId}")]
    [SwaggerOperation(Summary = "Sends message to show any media on the round 1 big board.")]
    public async Task<ActionResult> ShowMediaAsync(int questionId)
    {
        await _eventHub.Clients.All.SendAsync("round1ShowMedia", questionId);
        return Ok();
    }

    #endregion
}
