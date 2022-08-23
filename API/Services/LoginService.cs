using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text;
using GeekOff.Data;
using GeekOff.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace GeekOff.Services
{
    public class LoginService : ILoginService
    {
        private readonly ContextGo _contextGo;
        private readonly ILogger<LoginService> _logger;
        private readonly AppSettings _appSettings;
        public LoginService(ILogger<LoginService> logger, ContextGo context, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _contextGo = context;
            _appSettings = appSettings.Value;
        }

        public async Task<BearerDto> PlayerLoginAsync(string yEvent, Guid teamGuid)
        {
            var loginInfo = await _contextGo.Teamreference.FirstOrDefaultAsync(u => u.TeamGuid == teamGuid && u.Yevent == yEvent);
            if (loginInfo is null)
            {
                return null;
            }

            // we only care about the team level here. If we need player level we can do that later.

            loginInfo.LoginTime = DateTime.UtcNow;
            _contextGo.Teamreference.Update(loginInfo);
            await _contextGo.SaveChangesAsync();

            var returnUser = new BearerDto()
            {
                TeamNum = loginInfo.TeamNum,
                TeamName = loginInfo.Teamname,
                BearerToken = GenerateToken(teamGuid, false, loginInfo.TeamNum, null)
            };

            return returnUser;
        }

        // using our home grown AD for login for admins
        // This way we can avoid passwords and let MS handle that.
        public async Task<BearerDto> AdminLoginAsync(string userName)
        {
            var loginInfo = await _contextGo.AdminUser.FirstOrDefaultAsync(u => u.Username == userName);
            if (loginInfo is null)
            {
                return null;
            }

            loginInfo.LoginTime = DateTime.UtcNow;
            _contextGo.AdminUser.Update(loginInfo);
            await _contextGo.SaveChangesAsync();

            // we don't care about the team now
            var returnAdmin = new BearerDto()
            {
                TeamNum = 0,
                UserName = loginInfo.Username,
                HumanName = loginInfo.AdminName,
                BearerToken = GenerateToken(new Guid(), true, 0, loginInfo.Username)
            };

            return returnAdmin;
        }

        private string GenerateToken(Guid sessionGuid, bool adminFlag, int teamId, string? adminUsername)
        {
            var appSecret = _appSettings.Secret!;
            var appSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSecret));

            var appIssuer = _appSettings.Issuer;
            var appAudience = _appSettings.Audience;

            var claims = new ClaimsIdentity(new Claim[]
            {
                new Claim("sessionId", sessionGuid.ToString()),
                new Claim("teamId", teamId.ToString())
            });

            if (adminFlag)
            {
                claims.AddClaims(new Claim[]
                {
                    new Claim("username", adminUsername),
                    new Claim(ClaimTypes.Role, "admin")
                });
            }

            if (!adminFlag)
            {
                claims.AddClaim(new Claim(ClaimTypes.Role, "player"));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(3),
                Issuer = appIssuer,
                Audience = appAudience,
                SigningCredentials = new SigningCredentials(appSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
