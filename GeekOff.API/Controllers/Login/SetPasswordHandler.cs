using Microsoft.AspNetCore.Identity;

namespace GeekOff.Handlers;

public class SetPasswordHandler
{
    public class Request : IRequest<ApiResponse<StringReturn>>
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<StringReturn>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<StringReturn>> Handle(Request request, CancellationToken cancellationToken)
        {
            var loginInfo = await _contextGo.AdminUser
                .FirstOrDefaultAsync(u => u.Username == request.UserName, cancellationToken: cancellationToken);

            if (loginInfo is null)
            {
                return ApiResponse<StringReturn>.NotFound();
            }

            var loginDto = new AdminLogin()
            {
                UserName = request.UserName,
                Password = request.Password
            };

            var passwordHasher = new PasswordHasher<AdminLogin>();
            var hashedPassword = passwordHasher.HashPassword(loginDto, request.Password);

            loginInfo.Password = hashedPassword;

            _contextGo.AdminUser.Update(loginInfo);
            await _contextGo.SaveChangesAsync(cancellationToken);

            var returnObj = new StringReturn() {
                Message = "The password was successfully saved."
            };

            return ApiResponse<StringReturn>.Success(returnObj);
        }
    }
}

