using MediatR;

namespace GeekOff.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginController(ILogger<LoginController> logger, IMediator mediator) : ControllerBase
    {
        private readonly ILogger<LoginController> _logger = logger;
        private readonly IMediator _mediator = mediator;

        [AllowAnonymous]
        [HttpPost("player")]
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
        [HttpPost("admin")]
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
        [HttpPost("geekomatic")]
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
