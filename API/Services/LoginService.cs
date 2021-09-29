using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Linq;
using GeekOff.Data;
using GeekOff.Models;

namespace GeekOff.Services
{
    public class LoginService: ILoginService
    {
        private readonly contextGo _contextGo;
        private readonly ILogger<LoginService> _logger;
        public LoginService(ILogger<LoginService> logger, contextGo context)
        {
            _logger = logger;
            _contextGo = context;
        }

        public async Task<UserInfoDto> Login(string emailAddr)
        {
            var loginInfo = await _contextGo.TeamUser.SingleOrDefaultAsync(u => u.Username == emailAddr);
            if (loginInfo is null)
            {
                return null;
            }

            if (loginInfo.AdminFlag == true)
            {
                // we don't care about the team now
                var returnAdmin = new UserInfoDto()
                {
                    UserName = loginInfo.Username,
                    TeamNum = 0,
                    Roles = new List<string>()
                    {
                        "Admin",
                        "Player"
                    }
                };

                return returnAdmin;
            }

            var returnUser = new UserInfoDto()
            {
                UserName = loginInfo.Username,
                TeamNum = loginInfo.TeamNo,
                PlayerNum = loginInfo.PlayerNum,
                Roles = new List<string>()
                {
                    "Player"
                }
            };

            return returnUser;
        }

    }
}