using Microsoft.AspNetCore.Identity;

namespace GeekOff.Handlers;

public class CreateAdminUserHandler
{
    public class Request : IRequest<ApiResponse<StringReturn>>
    {
        public string UserName { get; set; } = string.Empty;
        public string AdminName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<StringReturn>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<StringReturn>> Handle(Request request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Password))
            {
                return ApiResponse<StringReturn>.BadRequest();
            }

            var loginInfo = await _contextGo.AdminUser
                .FirstOrDefaultAsync(u => u.Username == request.UserName, cancellationToken: cancellationToken);

            if (loginInfo is not null)
            {
                return ApiResponse<StringReturn>.Conflict();
            }

            var newUserGuid = Guid.NewGuid();

            var loginDto = new AdminLogin()
            {
                UserName = request.UserName,
                Password = request.Password
            };

            var passwordHasher = new PasswordHasher<AdminLogin>();
            var hashedPassword = passwordHasher.HashPassword(loginDto, request.Password);

            var newUserInfo = new AdminUser()
            {
                Username = request.UserName,
                AdminName = request.AdminName,
                Password = hashedPassword,
                UserGuid = newUserGuid
            };

            await _contextGo.AdminUser.AddAsync(newUserInfo, cancellationToken);
            await _contextGo.SaveChangesAsync(cancellationToken);

            var returnObj = new StringReturn() {
                Message = "The user was successfully created."
            };

            return ApiResponse<StringReturn>.Success(returnObj);
        }
    }
}

