namespace GeekOff.Controllers
{
    [ApiController]
    [Route("api/eventstatus")]
    public class EventStatusController : ControllerBase
    {
        private readonly ILogger<EventStatusController> _logger;
        private readonly IManageEventService _manageEventService;
        private readonly ILoginService _loginService;

        public EventStatusController(ILogger<EventStatusController> logger, ILoginService loginService, IManageEventService manageEventService)
        {
            _logger = logger;
            _manageEventService = manageEventService;
            _loginService = loginService;
        }

        [HttpGet("currentEvent")]
        [SwaggerOperation(Summary = "Get the current event. Called as part of app.component.ts.")]
        public async Task<ActionResult<string>> GetCurrentEventAsync()
            => Ok(await _manageEventService.GetCurrentEventAsync());

        [Authorize(Roles = "admin, player")]
        [HttpGet("currentQuestion/{yEvent}")]
        [SwaggerOperation(Summary = "Get the current question. Called when round1/contestant loads.")]
        public async Task<ActionResult<CurrentQuestionDto>> GetCurrentQuestionAsync(string yEvent)
            => Ok(await _manageEventService.GetCurrentQuestion(yEvent));

        [Authorize(Roles = "admin")]
        [HttpPut("dollarAmount/{yEvent}/{teamNum}")]
        [SwaggerOperation(Summary = "Get current user and team info from database based on logged in user.")]
        public async Task<ActionResult<string>> UpdateFundAmountAsync(string yEvent, int teamNum, decimal? dollarAmount)
            => Ok(await _manageEventService.UpdateFundAmountAsync(yEvent, teamNum, dollarAmount));

        [AllowAnonymous]
        [HttpPut("login/player")]
        [SwaggerOperation(Summary = "Login based on QR code.")]
        public async Task<ActionResult<string>> PlayerLoginAsync(string yEvent, Guid teamGuid)
            => Ok(await _loginService.PlayerLoginAsync(yEvent, teamGuid));

        [AllowAnonymous]
        [HttpPut("login/admin")]
        [SwaggerOperation(Summary = "Login from admin user.")]
        public async Task<ActionResult<string>> AdminLoginAsync(string username)
            => Ok(await _loginService.AdminLoginAsync(username));

        [AllowAnonymous]
        [HttpPost("login/geekomatic")]
        [SwaggerOperation(Summary = "Login from GeekOMatic")]
        public async Task<ActionResult<string>> GeekLoginAsync(string token)
            => Ok(await _loginService.GeekOMaticLoginAsync(token));

    }
}
