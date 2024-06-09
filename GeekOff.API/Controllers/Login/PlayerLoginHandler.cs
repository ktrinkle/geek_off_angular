namespace GeekOff.Handlers;

public class PlayerLoginHandler
{
    public class Request : IRequest<ApiResponse<BearerDto>>
    {
        public string YEvent { get; init; } = string.Empty;
        public Guid TeamGuid { get; init; }
    }

    //public record PlayerLoginCommand(string YEvent, Guid TeamGuid) : IRequest<ApiResponse<BearerDto>>;

    public class Handler(ContextGo contextGo, ILoginService loginService) : IRequestHandler<Request, ApiResponse<BearerDto>>
    {
        private readonly ContextGo _contextGo = contextGo;
        private readonly ILoginService _loginService = loginService;

        public async Task<ApiResponse<BearerDto>> Handle(Request request, CancellationToken token)
        {
            var loginInfo = await _contextGo.Teamreference
                .FirstOrDefaultAsync(u => u.TeamGuid == request.TeamGuid
                    && u.Yevent == request.YEvent, cancellationToken: token);

            if (loginInfo is null)
            {
                return ApiResponse<BearerDto>.NotFound();
            }

            // we only care about the team level here. If we need player level we can do that later.
            // @TODO: this update potentially violates CQRS, move to another service?

            loginInfo.LoginTime = DateTime.UtcNow;
            _contextGo.Teamreference.Update(loginInfo);
            await _contextGo.SaveChangesAsync(token);

            var tokenCall = new LoginTokenRequest()
            {
                TeamNum = loginInfo.TeamNum,
                SessionGuid = request.TeamGuid,
                AdminFlag = false,
                AdminUsername = null
            };

            var returnUser = new BearerDto()
            {
                TeamNum = loginInfo.TeamNum,
                TeamName = loginInfo.Teamname,
                BearerToken = await _loginService.GenerateTokenAsync(tokenCall)
            };

            return ApiResponse<BearerDto>.Success(returnUser);
        }
    }
}
