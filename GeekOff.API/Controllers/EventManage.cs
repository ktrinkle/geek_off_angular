using MediatR;

namespace GeekOff.Controllers;

[ApiController]
[Route("api/eventmanage")]
public class EventManageController(ILogger<EventManageController> logger, IManageEventService manageEventService,
        ITeamService teamService, IMediator mediator) : ControllerBase
{
    private readonly ILogger<EventManageController> _logger = logger;
    private readonly IManageEventService _manageEventService = manageEventService;
    private readonly ITeamService _teamService = teamService;
    private readonly IMediator _mediator = mediator;

    [Authorize(Roles = "admin")]
    [HttpGet("eventList")]
    [SwaggerOperation(Summary = "Get a list of all events.")]
    public async Task<IActionResult> GetEventListAsync([FromRoute] EventListHandler.Request request) =>
        await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            _ => throw new InvalidOperationException()
        };

    [Authorize(Roles = "admin")]
    [HttpPost("addEvent")]
    [SwaggerOperation(Summary = "Add a new event.")]
    public async Task<IActionResult> AddEventAsync([FromRoute] AddEventHandler.Request request) =>
        await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            { Status: QueryStatus.Conflict } result => Conflict(result.Value),
            _ => throw new InvalidOperationException()
        };

    [Authorize(Roles = "admin")]
    [HttpPut("setEvent/{YEvent}")]
    [SwaggerOperation(Summary = "Set the current event. Requires yEvent to be populated.")]
    public async Task<IActionResult> SetCurrentEventAsync([FromRoute] SetEventHandler.Request request) =>
        await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            { Status: QueryStatus.NotFound } result => NotFound(result.Value),
            _ => throw new InvalidOperationException()
        };

    [Authorize(Roles = "admin")]
    [HttpPut("dollarAmount/{YEvent}/{TeamNum}")]
    [SwaggerOperation(Summary = "Update current fund amount for team and event.")]
    public async Task<IActionResult> UpdateFundAmtAsync([FromRoute] UpdateFundAmtHandler.Request request) =>
        await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            { Status: QueryStatus.NotFound } result => NotFound(result.Value),
            _ => throw new InvalidOperationException()
        };

    [Authorize(Roles = "admin")]
    [HttpPut("cleanEvent/{YEvent}")]
    [SwaggerOperation(Summary = "Clean all results out of system for this event.")]
    public async Task<IActionResult> ResetEventAsync([FromRoute] ResetEventHandler.Request request) =>
        await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            { Status: QueryStatus.NotFound } result => NotFound(result.Value),
            _ => throw new InvalidOperationException()
        };

    [Authorize(Roles = "admin")]
    [HttpPut("createTeam/{YEvent}/{TeamName}")]
    [SwaggerOperation(Summary = "Create a new team for the event")]
    public async Task<IActionResult> AddNewEventTeamAsync([FromRoute] AddTeamHandler.Request request) =>
        await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            _ => throw new InvalidOperationException()
        };

    [Authorize(Roles = "admin")]
    [HttpPut("moveTeam/{YEvent}")]
    [SwaggerOperation(Summary = "Change a team number in the event")]
    public async Task<ActionResult<ApiResponse>> MoveTeamNumberAsync([FromRoute] MoveTeamHandler.Request request) =>
        await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            { Status: QueryStatus.Conflict } result => Conflict(result.Value),
            { Status: QueryStatus.NotFound } result => NotFound(result.Value),
            _ => throw new InvalidOperationException()
        };

    [Authorize(Roles = "admin")]
    [HttpGet("listTeamAndLink/{yEvent}")]
    [SwaggerOperation(Summary = "List teams and get links for all teams in an event.")]
    public async Task<ActionResult<List<NewTeamEntry>>> GetTeamListAsync(string yEvent)
        => Ok(await _teamService.GetTeamListAsync(yEvent));

}

