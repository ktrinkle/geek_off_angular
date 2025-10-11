using Microsoft.AspNetCore.Identity;

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

        public async Task<ApiResponse<BearerDto>> Handle(Request request, CancellationToken cancellationToken)
        {
            var loginInfo = await _contextGo.AdminUser
                .FirstOrDefaultAsync(u => u.Username == request.UserLogin.UserName, cancellationToken: cancellationToken);

            if (loginInfo is null)
            {
                return ApiResponse<BearerDto>.NotFound();
            }

            if (loginInfo.Password is null)
            {
                return ApiResponse<BearerDto>.BadRequest();
            }

            var passwordHasher = new PasswordHasher<AdminLogin>();
            var passwordTest = passwordHasher.VerifyHashedPassword(request.UserLogin, loginInfo.Password, request.UserLogin.Password);

            if (passwordTest is PasswordVerificationResult.Failed)
            {
                return ApiResponse<BearerDto>.BadRequest();
            }

            if (passwordTest is PasswordVerificationResult.SuccessRehashNeeded)
            {
                // recreate password
                var rehashedPassword = passwordHasher.HashPassword(request.UserLogin, request.UserLogin.Password);
                loginInfo.Password = rehashedPassword;
            }

            loginInfo.LoginTime = DateTime.UtcNow;
            _contextGo.AdminUser.Update(loginInfo);
            await _contextGo.SaveChangesAsync(cancellationToken);

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

