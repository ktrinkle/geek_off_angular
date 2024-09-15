namespace GeekOff.Controllers;

[ApiController]
[Route("api/eventstatus")]
public class EventStatusController(ILogger<EventStatusController> logger, IMediator mediator) : ControllerBase
{
    private readonly ILogger<EventStatusController> _logger = logger;
    private readonly IMediator _mediator = mediator;

    [HttpGet("currentEvent")]
    [SwaggerOperation(Summary = "Get the current event. Called as part of app.component.ts.")]
    public async Task<string> GetCurrentEventAsync([FromRoute] GetCurrentEventHandler.Request request) =>
        await _mediator.Send(request);

    [Authorize(Roles = "admin, player")]
    [HttpGet("currentQuestion/{YEvent}")]
    [SwaggerOperation(Summary = "Get the current question. Called when round1/contestant loads.")]
    public async Task<ActionResult<CurrentQuestionDto>> GetCurrentQuestionAsync([FromRoute] GetCurrentQuestionHandler.Request request) =>
        await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            { Status: QueryStatus.NoContent } => NoContent(),
            _ => throw new InvalidOperationException()
        };

    [Authorize(Roles = "admin")]
    [HttpGet("teamStats/{YEvent}")]
    [SwaggerOperation(Summary = "Get the statistics for the event")]
    public async Task<ActionResult<List<Round23Scores>>> GetTeamStatsAsync([FromRoute] TeamStatsHandler.Request request) =>
        await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            { Status: QueryStatus.NoContent } => NoContent(),
            _ => throw new InvalidOperationException()
        };
}

