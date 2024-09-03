namespace GeekOff.Controllers;

[ApiController]
[Route("api/round2_feud")]
public class Round2FeudController(ILogger<Round2FeudController> logger,
                        IHubContext<EventHub> eventHub, IMediator mediator) : ControllerBase
{

    private readonly ILogger<Round2FeudController> _logger = logger;
    private readonly IHubContext<EventHub> _eventHub = eventHub;
    private readonly IMediator _mediator = mediator;

    [Authorize(Roles = "admin")]
    [HttpGet("allSurvey/{YEvent}")]
    [SwaggerOperation(Summary = "Get all of the survey questions and answers for use of the operators.")]
    public async Task<ActionResult<List<Round2SurveyList>>> GetRound2SurveyMasterAsync([FromRoute] RoundTwoSurveyAnswerHandler.Request request)
        => await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            { Status: QueryStatus.NotFound } result => NotFound(result.Value),
            _ => throw new InvalidOperationException()
        };

    [Authorize(Roles = "admin")]
    [HttpGet("allQuestions/{YEvent}")]
    [SwaggerOperation(Summary = "Get all of the survey questions for use of the host.")]
    public async Task<ActionResult<List<Round2SurveyList>>> GetRound2QuestionListAsync([FromRoute] RoundTwoSurveyQuestionHandler.Request request)
        => await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            { Status: QueryStatus.NotFound } result => NotFound(result.Value),
            _ => throw new InvalidOperationException()
        };

    [Authorize(Roles = "admin")]
    [HttpGet("bigBoard/{YEvent}/{TeamNum}")]
    [SwaggerOperation(Summary = "Returns the big board for round 2")]
    public async Task<ActionResult<Round2BoardDto>> GetRound2DisplayBoardAsync([FromRoute] RoundTwoDisplayBoardHandler.Request request)
        => await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            { Status: QueryStatus.NotFound } result => NotFound(result.Value),
            _ => throw new InvalidOperationException()
        };

    [Authorize(Roles = "admin")]
    [HttpPost("teamanswer/text")]
    [SwaggerOperation(Summary = "Saves the team answer with points")]
    public async Task<ActionResult<string>> SetRound2AnswerTextAsync([FromBody] RoundTwoAnswerTextHandler.Request request)
    {
        var returnStatus = await _mediator.Send(request);

        switch (returnStatus.Status)
        {
            case QueryStatus.Success:
                await _eventHub.Clients.All.SendAsync("round2Answer");
                return Ok(returnStatus.Value);
            case QueryStatus.NotFound:
                return NotFound(returnStatus.Value);
            default:
                return BadRequest(returnStatus.Value);
        }
    }

    [Authorize(Roles = "admin")]
    [HttpPost("teamanswer/survey")]
    [SwaggerOperation(Summary = "Saves the team answer from a direct match to the survey")]
    public async Task<ActionResult<string>> SetRound2AnswerSurveyAsync([FromForm] RoundTwoAnswerSurveyHandler.Request request)
    {
        var returnStatus = await _mediator.Send(request);

        switch (returnStatus.Status)
        {
            case QueryStatus.Success:
                await _eventHub.Clients.All.SendAsync("round2Answer");
                return Ok(returnStatus.Value);
            case QueryStatus.NotFound:
                return NotFound(returnStatus.Value);
            default:
                return BadRequest(returnStatus.Value);
        }
    }

    [Authorize(Roles = "admin")]
    [HttpGet("scoreboard/{yEvent}")]
    [SwaggerOperation(Summary = "Returns the scoreboard for round 2")]
    public async Task<ActionResult<Round23Scores>> GetRound23ScoresAsync(string yEvent)
    {
        GetScoresHandler.Request request = new ()
        {
            YEvent = yEvent,
            RoundNum = 2,
            MaxRnk = 6
        };

        return await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            { Status: QueryStatus.BadRequest } result => BadRequest(),
            _ => throw new InvalidOperationException()
        };
    }

    [Authorize(Roles = "admin")]
    [HttpGet("firstPlayersAnswers/{YEvent}/{TeamNum}")]
    [SwaggerOperation(Summary = "Returns the first Players answers for round 2")]
    public async Task<ActionResult<Round23Scores>> GetFirstPlayersAnswersAsync([FromRoute] RoundTwoFirstPlayerHandler.Request request)
        => await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            { Status: QueryStatus.NotFound } result => NotFound(),
            { Status: QueryStatus.BadRequest } result => BadRequest(),
            _ => throw new InvalidOperationException()
        };

    [Authorize(Roles = "admin")]
    [HttpPut("finalize/{yEvent}")]
    [SwaggerOperation(Summary = "Finalize round 2")]
    public async Task<ActionResult<string>> FinalizeRoundAsync(string yEvent)
    {
        FinalizeRoundHandler.Request request = new ()
        {
            YEvent = yEvent,
            RoundNum = 2
        };

        return await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value.Message),
            { Status: QueryStatus.NotFound } result => NotFound(result.Value.Message),
            { Status: QueryStatus.BadRequest } result => BadRequest(result.Value.Message),
            _ => throw new InvalidOperationException()
        };
    }

    #region SignalR

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

    #endregion
}
