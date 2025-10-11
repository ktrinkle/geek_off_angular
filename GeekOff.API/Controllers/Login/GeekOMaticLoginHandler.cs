namespace GeekOff.Handlers;

public class GeekOMaticLoginHandler
{
    public class Request : IRequest<ApiResponse<BearerDto>>
    {
        public string Token { get; init; } = string.Empty;
    }

    public class Handler(ILoginService loginService) : IRequestHandler<Request, ApiResponse<BearerDto>>
    {
        private readonly ILoginService _loginService = loginService;

        public async Task<ApiResponse<BearerDto>> Handle(Request request, CancellationToken token)
        {
            var successfulGOM = await _loginService.GetGeekOMaticUserAsync(request.Token);

            if (!successfulGOM)
            {
                return ApiResponse<BearerDto>.BadRequest();
            }

            var tokenCall = new LoginTokenRequest()
            {
                TeamNum = 0,
                AdminFlag = false,
                AdminUsername = "GeekOMatic",
                SessionGuid = new Guid()
            };

            var returnGOM = new BearerDto()
            {
                TeamNum = 0,
                UserName = "GeekOMatic",
                HumanName = "GeekOMatic",
                BearerToken = await _loginService.GenerateTokenAsync(tokenCall)
            };

            return ApiResponse<BearerDto>.Success(returnGOM);
        }
    }
}

