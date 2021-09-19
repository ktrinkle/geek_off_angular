using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
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

        public async Task<string> Login(string emailAddr)
        {
            // this is not yet fleshed out but will need to take the email and compare against db
            // ultimately we need a JWT with the tokens for team, player #, role

            var loginInfo = await _contextGo.TeamUser.SingleOrDefaultAsync(u => u.Username == emailAddr);
            if (loginInfo is null)
            {
                return null;
            }

            if (loginInfo.AdminFlag == true)
            {
                // we don't care about the team now
                return "admin";
            }

            return loginInfo.TeamNo.ToString();
        }

    }
}