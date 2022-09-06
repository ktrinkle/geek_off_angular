using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Text;
using GeekOff.Data;
using GeekOff.Entities;
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
                BearerToken = await GenerateTokenAsync(teamGuid, false, loginInfo.TeamNum, null)
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
                BearerToken = await GenerateTokenAsync(Guid.NewGuid(), true, 0, loginInfo.Username)
            };

            return returnAdmin;
        }

        public async Task<BearerDto> GeekOMaticLoginAsync(string token)
        {
            if (await GetGeekOMaticUserAsync(token))
            {
                return new BearerDto()
                {
                    TeamNum = 0,
                    UserName = "GeekOMatic",
                    HumanName = "GeekOMatic",
                    BearerToken = await GenerateTokenAsync(Guid.NewGuid(), false, 0, "GeekOMatic")
                };
            }

            return null;
        }

        public async Task<int> GetSessionIdAsync(Guid? teamGuid)
        {
            var teamEntry = await _contextGo.Teamreference.FirstOrDefaultAsync(tr => tr.TeamGuid == teamGuid);

            if (teamEntry is null)
            {
                return 0;
            }

            return teamEntry.TeamNum;
        }


        public async Task<string?> GetAdminUserAsync(string userName)
        {
            var adminUser = await _contextGo.AdminUser.FirstOrDefaultAsync(tr => tr.Username == userName);

            if (adminUser is null)
            {
                return null;
            }

            return adminUser.AdminName;

        }

        public async Task<bool> GetGeekOMaticUserAsync(string token)
        {
            var geekOMaticUser = Encoding.UTF8.GetBytes(_appSettings.GeekOMaticUser);
            using var alg = SHA512.Create();

            var hashValue = alg.ComputeHash(geekOMaticUser).Aggregate("", (current, x) => current + $"{x:x2}");

            if (hashValue != token)
            {
                return false;
            }

            return true;

        }

        private async Task<string> GenerateTokenAsync(Guid sessionGuid, bool adminFlag, int teamId, string? adminUsername)
        {
            var appSecret = Encoding.UTF8.GetBytes(_appSettings.Secret!);
            var appSecurityKey = new SymmetricSecurityKey(appSecret);
            appSecurityKey.KeyId = _appSettings.JWTKeyId;

            var appIssuer = _appSettings.Issuer;
            var appAudience = _appSettings.Audience;

            var claims = new ClaimsIdentity(new Claim[]
            {
                new Claim("sessionid", sessionGuid.ToString()),
                new Claim("teamnum", teamId.ToString()),
                new Claim("username", adminUsername ?? ""),
            });

            if (adminFlag)
            {
                claims.AddClaim(new Claim(ClaimTypes.Role, "admin"));
                var realname = await _contextGo.AdminUser.FirstOrDefaultAsync(au => au.Username == adminUsername);
                claims.AddClaim(new Claim("realname", realname.AdminName));
            }

            if (!adminFlag && adminUsername != "GeekOMatic")
            {
                claims.AddClaim(new Claim(ClaimTypes.Role, "player"));
                claims.AddClaim(new Claim("realname", ""));
            }

            if (adminUsername == "GeekOMatic")
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
}
