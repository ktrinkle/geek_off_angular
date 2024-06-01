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
    public async Task<ActionResult<ApiResponse>> SetCurrentEventAsync([FromRoute] SetEventHandler.Request request) =>
        await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            { Status: QueryStatus.NotFound } result => NotFound(result.Value),
            _ => throw new InvalidOperationException()
        };


    [Authorize(Roles = "admin")]
    [HttpPut("dollarAmount/{yEvent}/{teamNum}")]
    [SwaggerOperation(Summary = "Get current user and team info from database based on logged in user.")]
    public async Task<ActionResult<string>> UpdateFundAmountAsync(string yEvent, int teamNum, decimal? dollarAmount)
        => Ok(await _manageEventService.UpdateFundAmountAsync(yEvent, teamNum, dollarAmount));

    [Authorize(Roles = "admin")]
    [HttpPut("cleanEvent/{yEvent}")]
    [SwaggerOperation(Summary = "Clean all results out of system for this event.")]
    public async Task<ActionResult<string>> ResetEventAsync(string yEvent)
        => Ok(await _manageEventService.ResetEvent(yEvent));

    [Authorize(Roles = "admin")]
    [HttpPut("createTeam/{yEvent}/{teamName}")]
    [SwaggerOperation(Summary = "Create a new team for the event")]
    public async Task<ActionResult<NewTeamEntry>> AddNewEventTeamAsync(string yEvent, string? teamName)
        => Ok(await _teamService.AddNewEventTeamAsync(yEvent, teamName));

    [Authorize(Roles = "admin")]
    [HttpPut("moveTeam/{yEvent}/{teamName}")]
    [SwaggerOperation(Summary = "Change a team number in the event")]
    public async Task<ActionResult<ApiResponse>> MoveTeamNumberAsync(string yEvent, int oldTeamNum, int newTeamNum)
        => Ok(await _teamService.MoveTeamNumberAsync(yEvent, oldTeamNum, newTeamNum));

    [Authorize(Roles = "admin")]
    [HttpGet("listTeamAndLink/{yEvent}")]
    [SwaggerOperation(Summary = "List teams and get links for all teams in an event.")]
    public async Task<ActionResult<List<NewTeamEntry>>> GetTeamListAsync(string yEvent)
        => Ok(await _teamService.GetTeamListAsync(yEvent));

}

