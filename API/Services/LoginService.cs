using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GeekOff.Data;
using GeekOff.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GeekOff.Services
{
    public class LoginService : ILoginService
    {
        private readonly ContextGo _contextGo;
        private readonly ILogger<LoginService> _logger;
        public LoginService(ILogger<LoginService> logger, ContextGo context)
        {
            _logger = logger;
            _contextGo = context;
        }

        public async Task<UserInfoDto> Login(string userId)
        {
            var loginInfo = await _contextGo.TeamUser.FirstOrDefaultAsync(u => u.Username == userId);
            if (loginInfo is null)
            {
                return null;
            }

            loginInfo.LoginTime = DateTime.UtcNow;
            _contextGo.TeamUser.Update(loginInfo);
            await _contextGo.SaveChangesAsync();

            if (loginInfo.AdminFlag == true)
            {
                // we don't care about the team now
                var returnAdmin = new UserInfoDto()
                {
                    PlayerName = loginInfo.PlayerName,
                    UserName = loginInfo.Username,
                    TeamNum = 0
                };

                return returnAdmin;
            }

            var returnUser = new UserInfoDto()
            {
                PlayerName = loginInfo.PlayerName,
                UserName = loginInfo.Username,
                TeamNum = loginInfo.TeamNo,
                PlayerNum = loginInfo.PlayerNum
            };

            return returnUser;
        }

    }
}
