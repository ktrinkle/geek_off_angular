using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeekOff.Entities;
using GeekOff.Models;
using GeekOff.Services;

namespace GeekOff.Extensions
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

    public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
    {
        _next = next;
        _appSettings = appSettings.Value;
    }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userJwtDto = await ValidateJwtTokenAsync(token);
            if (userJwtDto != null && userJwtDto.TeamNum != 0)
            {
                // attach user to context on successful jwt validation
                context.Items["User"] = userJwtDto.TeamNum;
            }

            if (userJwtDto != null && userJwtDto.UserName != "")
            {
                // attach user to context on successful jwt validation
                context.Items["Name"] = userJwtDto.AdminName;
            }

            await _next(context);
        }

        public async Task<JWTDto?> ValidateJwtTokenAsync(string token)
        {
            if (token == null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            try
            {
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
                // we crash here...saying it's unvalidated. Need to confirm this.
                var teamSessionGuid = jwtToken.Claims.First(x => x.Type == "sessionid").Value ?? "";
                var username = jwtToken.Claims.First(x => x.Type == "username").Value ?? "";
                var adminName = jwtToken.Claims.First(x => x.Type == "realname").Value ?? "";
                var teamNumStr = jwtToken.Claims.First(x => x.Type == "teamnum").Value ?? "0";

                if (!int.TryParse(teamNumStr, out var teamNum))
                {
                    teamNum = 0;
                }

                // return user id from JWT token if validation successful
                return new JWTDto() {
                    TeamNum = teamNum,
                    UserName = username,
                    SessionGuid = Guid.Parse(teamSessionGuid),
                    AdminName = adminName
                };
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
    }
}
