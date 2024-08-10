namespace GeekOff.Controllers;

[ApiController]
[Route("api/round3")]
public class Round3Controller(ILogger<Round3Controller> logger, IManageEventService manageEventService,
                        IHubContext<EventHub> eventHub, IMediator mediator) : ControllerBase
{

    private readonly ILogger<Round3Controller> _logger = logger;
    private readonly IManageEventService _manageEventService = manageEventService;
    private readonly IHubContext<EventHub> _eventHub = eventHub;
    private readonly IMediator _mediator = mediator;

    [Authorize(Roles = "admin")]
    [HttpGet("allQuestions/{yEvent}")]
    [SwaggerOperation(Summary = "Get all of the questions and points for use of the operators.")]
    public async Task<ActionResult<List<Round3QuestionDto>>> GetRound3MasterAsync(string yEvent)
        => Ok(await _manageEventService.GetRound3Master(yEvent));

    [Authorize(Roles = "admin")]
    [HttpGet("allTeams/{yEvent}")]
    [SwaggerOperation(Summary = "Get all of the round 3 teams.")]
    public async Task<ActionResult<List<IntroDto>>> GetRound3TeamsAsync(string yEvent)
        => Ok(await _manageEventService.GetRound3Teams(yEvent));

    [Authorize(Roles = "admin")]
    [HttpPost("teamanswer")]
    [SwaggerOperation(Summary = "Saves the team answer with points")]
    public async Task<ActionResult<string>> SetRound3AnswerAsync(List<Round3AnswerDto> submitAnswer)
    {
        var returnVar = await _manageEventService.SetRound3Answer(submitAnswer);
        await _eventHub.Clients.All.SendAsync("round3ScoreUpdate");
        return Ok(returnVar);
    }

    [Authorize(Roles = "admin")]
    [HttpGet("scoreboard/{yEvent}")]
    [SwaggerOperation(Summary = "Returns the scoreboard for round 3")]
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
    [HttpGet("updateScoreboard")]
    [SwaggerOperation(Summary = "Sends message to update the scoreboard.")]
    public async Task<ActionResult> UpdateScoreboardAsync()
    {
        // add in controller here
        await _eventHub.Clients.All.SendAsync("round3ScoreUpdate");
        return Ok();
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
}
