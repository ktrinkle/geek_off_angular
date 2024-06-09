using MediatR;

namespace GeekOff.Controllers
{
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

        [AllowAnonymous]
        [HttpPost("login/player")]
        [SwaggerOperation(Summary = "Login based on QR code.")]
        public async Task<ActionResult<BearerDto>> PlayerLoginAsync(PlayerLoginHandler.Request request) =>
            await _mediator.Send(request) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                { Status: QueryStatus.NotFound } => NotFound(),
                { Status: QueryStatus.NoContent } => NoContent(),
                _ => throw new InvalidOperationException()
            };

        [AllowAnonymous]
        [HttpPost("login/admin")]
        [SwaggerOperation(Summary = "Login from admin user.")]
        public async Task<ActionResult<BearerDto>> AdminLoginAsync(AdminLoginHandler.Request request) =>
            await _mediator.Send(request) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                { Status: QueryStatus.NotFound } => NotFound(),
                { Status: QueryStatus.NoContent } => NoContent(),
                _ => throw new InvalidOperationException()
            };

        [AllowAnonymous]
        [HttpPost("login/geekomatic")]
        [SwaggerOperation(Summary = "Login from GeekOMatic")]
        public async Task<ActionResult<BearerDto>> GeekLoginAsync([FromRoute] GeekOMaticLoginHandler.Request request) =>
            await _mediator.Send(request) switch
            {
                { Status: QueryStatus.Success } result => Ok(result.Value),
                { Status: QueryStatus.NotFound } => NotFound(),
                { Status: QueryStatus.NoContent } => NoContent(),
                _ => throw new InvalidOperationException()
            };

    }
}
