namespace GeekOff.Controllers;

[ApiController]
[Route("api/round3")]
public class Round3Controller(ILogger<Round3Controller> logger,
                        IHubContext<EventHub> eventHub, IMediator mediator) : ControllerBase
{
    private readonly ILogger<Round3Controller> _logger = logger;
    private readonly IHubContext<EventHub> _eventHub = eventHub;
    private readonly IMediator _mediator = mediator;

    [Authorize(Roles = "admin")]
    [HttpGet("bigDisplay/{yEvent}")]
    [SwaggerOperation(Summary = "Get list of the round 3 questions for the big board with media.")]
    public async Task<ActionResult<List<Round13QuestionDisplay>>> GetRound3QuestionsAsync(string yEvent)
    {
        GetQuestionHandler.Request request = new ()
        {
            YEvent = yEvent,
            RoundNum = 3
        };

        return await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            { Status: QueryStatus.NotFound } => NotFound(),
            { Status: QueryStatus.BadRequest } => BadRequest(),
            _ => throw new InvalidOperationException()
        };
    }

    [Authorize(Roles = "admin")]
    [HttpGet("allQuestions/{YEvent}")]
    [SwaggerOperation(Summary = "Get all of the questions and points for use of the operators.")]
    public async Task<ActionResult<List<Round3QuestionDto>>> GetRound3MasterAsync([FromRoute] RoundThreeQuestionOperatorHandler.Request request)
        => await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            { Status: QueryStatus.NotFound } => NotFound(),
            { Status: QueryStatus.BadRequest } => BadRequest(),
            _ => throw new InvalidOperationException()
        };

    [Authorize(Roles = "admin")]
    [HttpGet("allTeams/{YEvent}")]
    [SwaggerOperation(Summary = "Get all of the round 3 teams.")]
    public async Task<ActionResult<List<IntroDto>>> GetRound3TeamsAsync([FromRoute] RoundThreeTeamListHandler.Request request)
        => await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            { Status: QueryStatus.NotFound } => NotFound(),
            { Status: QueryStatus.BadRequest } => BadRequest(),
            _ => throw new InvalidOperationException()
        };

    [Authorize(Roles = "admin")]
    [HttpPost("teamanswer")]
    [SwaggerOperation(Summary = "Saves the team answer with points")]
    public async Task<ActionResult<string>> SetRound3AnswerAsync([FromForm] RoundThreeTeamAnswerHandler.Request request)
    {
        await _eventHub.Clients.All.SendAsync("round3ScoreUpdate");

        return await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value.Message),
            { Status: QueryStatus.NotFound } => NotFound(),
            { Status: QueryStatus.BadRequest } result => BadRequest(result.Value.Message),
            _ => throw new InvalidOperationException()
        };
    }

    [Authorize(Roles = "admin")]
    [HttpGet("scoreboard/{yEvent}")]
    [SwaggerOperation(Summary = "Returns the scoreboard for round 3")]
    public async Task<ActionResult<Round23Scores>> GetRound23ScoresAsync(string yEvent)
    {
        GetScoresHandler.Request request = new ()
        {
            YEvent = yEvent,
            RoundNum = 3,
            MaxRnk = 3
        };

        return await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            { Status: QueryStatus.BadRequest } result => BadRequest(),
            _ => throw new InvalidOperationException()
        };
    }

    [Authorize(Roles = "admin")]
    [HttpPut("finalize/{yEvent}")]
    [SwaggerOperation(Summary = "Finalize round 3")]
    public async Task<ActionResult<string>> FinalizeRoundAsync(string yEvent)
    {
        FinalizeRoundHandler.Request request = new ()
        {
            YEvent = yEvent,
            RoundNum = 3
        };

        return await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value.Message),
            { Status: QueryStatus.NotFound } result => NotFound(result.Value.Message),
            { Status: QueryStatus.BadRequest } result => BadRequest(result.Value.Message),
            _ => throw new InvalidOperationException()
        };
    }

    // future controller - reset board state since we have this in the DB, in case we get out of sync.

    #region SignalR

    [Authorize(Roles = "admin")]
    [HttpGet("updateScoreboard")]
    [SwaggerOperation(Summary = "Sends message to update the scoreboard.")]
    public async Task<ActionResult> UpdateScoreboardAsync()
    {
        await _eventHub.Clients.All.SendAsync("round3ScoreUpdate");
        return Ok();
    }

    [Authorize(Roles = "admin")]
    [HttpGet("bigboard/animate")]
    [SwaggerOperation(Summary = "Send message to animate big board.")]
    public async Task<ActionResult> AnimateBigBoardAsync()
    {
        await _eventHub.Clients.All.SendAsync("round3Animate");
        return Ok();
    }

    [Authorize(Roles = "admin")]
    [HttpGet("bigboard/show")]
    [SwaggerOperation(Summary = "Show the big board. Note that the state of the board is managed purely in the UI.")]
    public async Task<ActionResult> ShowBigBoardAsync()
    {
        await _eventHub.Clients.All.SendAsync("round3BigBoard");
        return Ok();
    }

    [Authorize(Roles = "admin")]
    [HttpGet("bigboard/reveal/{entryNum}")]
    [SwaggerOperation(Summary = "Send message to reveal big board answer.")]
    public async Task<ActionResult> RevealAnswerAsync(int entryNum)
    {
        await _eventHub.Clients.All.SendAsync("round3AnswerShow", entryNum);
        return Ok();
    }

    [Authorize(Roles = "admin")]
    [HttpGet("bigboard/showFinalScreen/{screenNum}")]
    [SwaggerOperation(Summary = "Send message to show final question screen for final Jeopardy.")]
    public async Task<ActionResult> ShowFinalIntroScreenAsync(int screenNum)
    {
        await _eventHub.Clients.All.SendAsync("round3FinalShow", screenNum);
        return Ok();
    }

    [Authorize(Roles = "admin")]
    [HttpGet("bigboard/playFinalMusic")]
    [SwaggerOperation(Summary = "Play final Jeopardy music.")]
    public async Task<ActionResult> PlayFinalJepMusicAsync()
    {
        await _eventHub.Clients.All.SendAsync("playFinalJepMusic");
        return Ok();
    }

    [Authorize(Roles = "admin")]
    [HttpGet("bigboard/showPrizeScreen/{screenNum}")]
    [SwaggerOperation(Summary = "Send message to show prize screen for final Jeopardy.")]
    public async Task<ActionResult> ShowFinalPrizeScreenAsync(int screenNum)
    {
        await _eventHub.Clients.All.SendAsync("round3FinalShow", screenNum);
        return Ok();
    }

    [Authorize(Roles = "admin")]
    [HttpGet("bigboard/startCredits")]
    [SwaggerOperation(Summary = "Send message to start the credit roll.")]
    public async Task<ActionResult> StartCreditAsync()
    {
        await _eventHub.Clients.All.SendAsync("round3FinalCredits");
        return Ok();
    }

    #endregion
}
