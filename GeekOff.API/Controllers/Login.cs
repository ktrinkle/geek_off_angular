namespace GeekOff.Controllers;

[ApiController]
[Route("api/login")]
public class LoginController(IMediator mediator) : ControllerBase
{
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
            { Status: QueryStatus.BadRequest } => BadRequest(),
            _ => throw new InvalidOperationException()
        };

    [AllowAnonymous]
    [HttpPost("geekomatic")]
    [SwaggerOperation(Summary = "Login from GeekOMatic")]
    public async Task<ActionResult<BearerDto>> GeekLoginAsync([FromBody] GeekOMaticLoginHandler.Request request) =>
        await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value),
            { Status: QueryStatus.NotFound } => NotFound(),
            { Status: QueryStatus.NoContent } => NoContent(),
            _ => throw new InvalidOperationException()
        };

    [AllowAnonymous]
    [HttpPost("admin/setPassword")]
    [SwaggerOperation(Summary = "Update a password for an existing user.", Description = "Requires the GUID of the user from the database.")]
    public async Task<ActionResult<string>> UpdatePasswordAsync([FromBody] SetPasswordHandler.Request request) =>
        await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value.Message),
            { Status: QueryStatus.NotFound } => NotFound(),
            _ => throw new InvalidOperationException()
        };

    [Authorize(Roles = "admin")]
    [HttpPost("admin/createUser")]
    [SwaggerOperation(Summary = "Create a new admin user.")]
    public async Task<ActionResult<string>> CreateNewAdminUserAsync([FromBody] CreateAdminUserHandler.Request request) =>
        await _mediator.Send(request) switch
        {
            { Status: QueryStatus.Success } result => Ok(result.Value.Message),
            { Status: QueryStatus.BadRequest } result => BadRequest(result.Value.Message),
            { Status: QueryStatus.Conflict } result => Conflict(result.Value.Message),
            _ => throw new InvalidOperationException()
        };
}
