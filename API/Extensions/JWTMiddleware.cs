using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Logging;
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

            IdentityModelEventSource.ShowPII = true;

            var tokenHandler = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var jwtToken = tokenHandler;
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
                "admin" => jwtToken.Claims.FirstOrDefault(x => x.Type == "realname").Value,
                "geekomatic" => null,
                "player" => null,
                _ => null
            };

            var teamNumStr = roleType switch
            {
                "admin" => "0",
                "geekomatic" => "0",
                "player" => jwtToken.Claims.FirstOrDefault(x => x.Type == "teamnum").Value,
                _ => "0"
            };

            var geekomatic = roleType switch
            {
                "admin" => false,
                "geekomatic" => jwtToken.Claims.FirstOrDefault(x => x.Type == "geekomatic").Value == "true" || false,
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
                AdminName = geekomatic == true ? "GeekOMatic" : adminName
            };
        }
    }
}
