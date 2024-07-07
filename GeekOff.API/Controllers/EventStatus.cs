namespace GeekOff.Controllers;

[ApiController]
[Route("api/eventstatus")]
public class EventStatusController(ILogger<EventStatusController> logger, IMediator mediator) : ControllerBase
{
    private readonly ILogger<EventStatusController> _logger = logger;
    private readonly IMediator _mediator = mediator;

    [Authorize]
    [HttpGet("currentEvent")]
    [SwaggerOperation(Summary = "Get the current event. Called as part of app.component.ts.")]
    public async Task<ActionResult<string>> GetCurrentEventAsync([FromRoute] GetCurrentEventHandler.Request request) =>
        await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            _ => throw new InvalidOperationException()
        };

    [Authorize(Roles = "admin, player")]
    [HttpGet("currentQuestion/{YEvent}")]
    [SwaggerOperation(Summary = "Get the current question. Called when round1/contestant loads.")]
    public async Task<ActionResult<CurrentQuestionDto>> GetCurrentQuestionAsync([FromRoute] GetCurrentQuestionHandler.Request request) =>
        await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            { Status: QueryStatus.NoContent } result => NoContent(),
            _ => throw new InvalidOperationException()
        };
}

