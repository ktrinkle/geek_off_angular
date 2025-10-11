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
    public async Task<ActionResult<string>> SetRound3AnswerAsync([FromBody] RoundThreeTeamAnswerHandler.Request request)
    {
        var saveStatus = await _mediator.Send(request) ?? throw new InvalidOperationException();

        if (saveStatus.Status == QueryStatus.Success)
        {
            var yEvent = request.Round3Answers[0].YEvent;

            RoundThreeGeekOMaticScoreHandler.Request newRequest = new ()
            {
                YEvent = yEvent
            };

            var sendStatus = await _mediator.Send(newRequest) ?? throw new InvalidOperationException();

            switch (sendStatus.Status)
            {
                case QueryStatus.Success:
                    foreach (var team in sendStatus.Value!)
                    {
                        await _eventHub.Clients.All.SendAsync("round3Score", team.TeamColor + team.TeamScore);
                    }
                    break;
                default:
                    break;
            }
        }

        return saveStatus switch
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
            { Status: QueryStatus.BadRequest } => BadRequest(),
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

    #region GeekOMatic

    [Authorize(Roles = "geekomatic")]
    [HttpGet("getTeamColors/{YEvent}")]
    [SwaggerOperation(Summary = "Saves the team answer with points")]
    public async Task<ActionResult<string>> GetRound3TeamColorsAsync([FromRoute] RoundThreeTeamColorHandler.Request request)
    {
        return await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            { Status: QueryStatus.NotFound } => NotFound(),
            { Status: QueryStatus.BadRequest } => BadRequest(),
            _ => throw new InvalidOperationException()
        };
    }

    [Authorize(Roles = "admin")]
    [HttpPut("buzzer/init")]
    [SwaggerOperation(Summary = "Sends message to initialize the buzzers.")]
    public async Task<ActionResult> InitBuzzerAsync()
    {
        await _eventHub.Clients.All.SendAsync("round3InitBuzzer");
        return Ok();
    }

    [Authorize(Roles = "admin")]
    [HttpPut("buzzer/unlock")]
    [SwaggerOperation(Summary = "Sends message to unlock the buzzers.")]
    public async Task<ActionResult> UnlockBuzzerAsync()
    {
        await _eventHub.Clients.All.SendAsync("round3Buzzer", "U");
        return Ok();
    }

    [Authorize(Roles = "admin, geekomatic")]
    [HttpPut("buzzer/answer/{teamColor}")]
    [SwaggerOperation(Summary = "Send message that a team answered the board.")]
    public async Task<ActionResult> BuzzerAnswerAsync(char teamColor)
    {
        await _eventHub.Clients.All.SendAsync("round3Buzzer", teamColor);
        // this will also lock out the other buzzers
        return Ok();
    }

    [Authorize(Roles = "admin")]
    [HttpPut("buzzer/lock")]
    [SwaggerOperation(Summary = "Send message to lock the buzzers.")]
    public async Task<ActionResult> BuzzerLockAsync(int teamNum)
    {
        await _eventHub.Clients.All.SendAsync("round3Buzzer", "L");
        return Ok();
    }

    [Authorize(Roles = "geekomatic")]
    [HttpGet("getTeamScores/{YEvent}")]
    [SwaggerOperation(Summary = "Saves the team answer with points")]
    public async Task<ActionResult<string>> GetRound3TeamScoresColorsAsync([FromRoute] RoundThreeGeekOMaticScoreHandler.Request request)
    {
        return await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            { Status: QueryStatus.NotFound } => NotFound(),
            { Status: QueryStatus.BadRequest } => BadRequest(),
            _ => throw new InvalidOperationException()
        };
    }

    #endregion
    #region SignalR

    [Authorize(Roles = "admin")]
    [HttpPut("updateScoreboard")]
    [SwaggerOperation(Summary = "Sends message to update the scoreboard.")]
    public async Task<ActionResult> UpdateScoreboardAsync()
    {
        await _eventHub.Clients.All.SendAsync("round3ScoreUpdate");
        return Ok();
    }

    [Authorize(Roles = "admin")]
    [HttpPut("bigboard/animate")]
    [SwaggerOperation(Summary = "Send message to animate big board.")]
    public async Task<ActionResult> AnimateBigBoardAsync()
    {
        await _eventHub.Clients.All.SendAsync("round3Animate");
        return Ok();
    }

    [Authorize(Roles = "admin")]
    [HttpPut("bigboard/show")]
    [SwaggerOperation(Summary = "Show the big board. Note that the state of the board is managed purely in the UI.")]
    public async Task<ActionResult> ShowBigBoardAsync()
    {
        await _eventHub.Clients.All.SendAsync("round3BigBoard");
        return Ok();
    }

    [Authorize(Roles = "admin")]
    [HttpPut("bigboard/reveal/{entryNum}")]
    [SwaggerOperation(Summary = "Send message to reveal big board answer.")]
    public async Task<ActionResult> RevealAnswerAsync(int entryNum)
    {
        await _eventHub.Clients.All.SendAsync("round3AnswerShow", entryNum);
        return Ok();
    }

    [Authorize(Roles = "admin")]
    [HttpPut("bigboard/showFinalScreen/{screenNum}")]
    [SwaggerOperation(Summary = "Send message to show final question screen for final Jeopardy.")]
    public async Task<ActionResult> ShowFinalIntroScreenAsync(int screenNum)
    {
        await _eventHub.Clients.All.SendAsync("round3FinalShow", screenNum);
        return Ok();
    }

    [Authorize(Roles = "admin")]
    [HttpPut("bigboard/playFinalMusic")]
    [SwaggerOperation(Summary = "Play final Jeopardy music.")]
    public async Task<ActionResult> PlayFinalJepMusicAsync()
    {
        await _eventHub.Clients.All.SendAsync("playFinalJepMusic");
        return Ok();
    }

    [Authorize(Roles = "admin")]
    [HttpPut("bigboard/showPrizeScreen/{screenNum}")]
    [SwaggerOperation(Summary = "Send message to show prize screen for final Jeopardy.")]
    public async Task<ActionResult> ShowFinalPrizeScreenAsync(int screenNum)
    {
        await _eventHub.Clients.All.SendAsync("round3FinalShow", screenNum);
        return Ok();
    }

    [Authorize(Roles = "admin")]
    [HttpPut("bigboard/startCredits")]
    [SwaggerOperation(Summary = "Send message to start the credit roll.")]
    public async Task<ActionResult> StartCreditAsync()
    {
        await _eventHub.Clients.All.SendAsync("round3FinalCredits");
        return Ok();
    }

    #endregion
}
