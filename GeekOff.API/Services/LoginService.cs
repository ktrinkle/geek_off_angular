namespace GeekOff.Services;

public class LoginService(ILogger<LoginService> logger, ContextGo context, IOptions<AppSettings> appSettings) : ILoginService
{
    private readonly ContextGo _contextGo = context;
    private readonly ILogger<LoginService> _logger = logger;
    private readonly AppSettings _appSettings = appSettings.Value;

    public async Task<int> GetSessionIdAsync(Guid? teamGuid)
    {
        var teamEntry = await _contextGo.Teamreference.FirstOrDefaultAsync(tr => tr.TeamGuid == teamGuid);

        return teamEntry is null ? 0 : teamEntry.TeamNum;
    }


    public async Task<string?> GetAdminUserAsync(string userName)
    {
        var adminUser = await _contextGo.AdminUser.FirstOrDefaultAsync(tr => tr.Username == userName);

        return adminUser?.AdminName;

    }

    public async Task<bool> GetGeekOMaticUserAsync(string token)
    {
        var geekOMaticUser = Encoding.UTF8.GetBytes(_appSettings.GeekOMaticUser);

        //var hashValue = alg.ComputeHash(geekOMaticUser).Aggregate("", (current, x) => current + $"{x:x2}");
        var hashValue = SHA512.HashData(geekOMaticUser).Aggregate("", (current, x) => current + $"{x:x2}");

        return hashValue == token;

    }

    public async Task<string> GenerateTokenAsync(LoginTokenRequest loginTokenRequest)
    {
        var appSecret = Encoding.UTF8.GetBytes(_appSettings.Secret!);
        var appSecurityKey = new SymmetricSecurityKey(appSecret) {KeyId = _appSettings.JWTKeyId};

        var appIssuer = _appSettings.Issuer;
        var appAudience = _appSettings.Audience;

        var claims = new ClaimsIdentity(
        [
            new Claim("sessionid", loginTokenRequest.SessionGuid.ToString()),
            new Claim("teamnum", loginTokenRequest.TeamNum.ToString()),
            new Claim("username", loginTokenRequest.AdminUsername ?? ""),
        ]);

        if (loginTokenRequest.AdminFlag)
        {
            claims.AddClaim(new Claim(ClaimTypes.Role, "admin"));
            var realname = await _contextGo.AdminUser.FirstOrDefaultAsync(au => au.Username == loginTokenRequest.AdminUsername);
            claims.AddClaim(new Claim("realname", realname!.AdminName!));
        }

        if (!loginTokenRequest.AdminFlag && loginTokenRequest.AdminUsername != "GeekOMatic")
        {
            claims.AddClaim(new Claim(ClaimTypes.Role, "player"));
            claims.AddClaim(new Claim("realname", ""));
        }

        if (loginTokenRequest.AdminUsername == "GeekOMatic")
        {
            claims.AddClaim(new Claim(ClaimTypes.Role, "geekomatic"));
            claims.AddClaim(new Claim("geekomatic", "true"));
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = appSecret;
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = DateTime.UtcNow.AddHours(4),
            Issuer = appIssuer,
            Audience = appAudience,
            SigningCredentials = new SigningCredentials(appSecurityKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
