namespace GeekOff.Handlers;

public class AdminLoginHandler
{
    public class Request : IRequest<ApiResponse<BearerDto>>
    {
        public AdminLogin UserLogin { get; init; } = new AdminLogin();
    }

    public class Handler(ContextGo contextGo, ILoginService loginService) : IRequestHandler<Request, ApiResponse<BearerDto>>
    {
        private readonly ContextGo _contextGo = contextGo;
        private readonly ILoginService _loginService = loginService;

        public async Task<ApiResponse<BearerDto>> Handle(Request request, CancellationToken token)
        {
            var loginInfo = await _contextGo.AdminUser
                .FirstOrDefaultAsync(u => u.Username == request.UserLogin.UserName, cancellationToken: token);

            if (loginInfo is null)
            {
                return ApiResponse<BearerDto>.NotFound();
            }

            // insert password handling here

            loginInfo.LoginTime = DateTime.UtcNow;
            _contextGo.AdminUser.Update(loginInfo);
            await _contextGo.SaveChangesAsync(token);

            var tokenCall = new LoginTokenRequest()
            {
                TeamNum = 0,
                AdminFlag = true,
                AdminUsername = loginInfo.Username,
                SessionGuid = Guid.NewGuid()
            };

            // we don't care about the team now
            var returnAdmin = new BearerDto()
            {
                TeamNum = 0,
                UserName = loginInfo.Username,
                HumanName = loginInfo.AdminName,
                BearerToken = await _loginService.GenerateTokenAsync(tokenCall)
            };

            return ApiResponse<BearerDto>.Success(returnAdmin);
        }
    }
}

