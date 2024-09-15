using Microsoft.IdentityModel.Logging;

namespace GeekOff.Extensions;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AppSettings _appSettings;

public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
{
    _next = next;
    _appSettings = appSettings.Value;
}

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
        var userJwtDto = await ValidateJwtTokenAsync(token ?? string.Empty);

        if (userJwtDto != null)
        {
            // attach user to context on successful jwt validation
            context.Items["User"] = userJwtDto.TeamNum;
            context.Items["Role"] = userJwtDto.Role;
        }

        if (userJwtDto != null && userJwtDto.UserName != "")
        {
            // attach user to context on successful jwt validation
            context.Items["Name"] = userJwtDto.AdminName;
        }

        await _next(context);
    }

    public async Task<JWTDto?> ValidateJwtTokenAsync(string? token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        IdentityModelEventSource.ShowPII = true;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_appSettings.Secret!);

        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key) { KeyId = _appSettings.JWTKeyId },
            ValidateIssuer = false,
            ValidIssuer = _appSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = _appSettings.Audience,
            // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
            ClockSkew = TimeSpan.Zero
        }, out var validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;

        // we have to go through conditional logic here
        var roleType = jwtToken.Claims.First(x => x.Type == "role").Value;

        var teamSessionGuid = jwtToken.Claims.First(x => x.Type == "sessionid").Value;

        var username = roleType switch
        {
            "admin" => jwtToken.Claims.First(x => x.Type == "username").Value,
            "geekomatic" => null,
            "player" => null,
            _ => null
        };

        var adminName = roleType switch
        {
            "admin" => jwtToken.Claims.FirstOrDefault(x => x.Type == "realname")!.Value,
            "geekomatic" => null,
            "player" => null,
            _ => null
        };

        var teamNumStr = roleType switch
        {
            "admin" => "0",
            "geekomatic" => "0",
            "player" => jwtToken.Claims.FirstOrDefault(x => x.Type == "teamnum")!.Value,
            _ => "0"
        };

        var geekomatic = roleType switch
        {
            "admin" => false,
            "geekomatic" => jwtToken.Claims.FirstOrDefault(x => x.Type == "geekomatic")!.Value == "true" || false,
            "player" => false,
            _ => false
        };

        if (!int.TryParse(teamNumStr, out var teamNum))
        {
            teamNum = 0;
        }

        // return user id from JWT token if validation successful
        return new JWTDto() {
            TeamNum = teamNum,
            UserName = geekomatic == true ? "GeekOMatic" : username,
            SessionGuid = Guid.Parse(teamSessionGuid),
            AdminName = geekomatic == true ? "GeekOMatic" : adminName,
            Role = roleType
        };
    }
}
