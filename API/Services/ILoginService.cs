using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;
using GeekOff.Models;

namespace GeekOff.Services
{
    public interface ILoginService
    {
        Task<UserInfoDto> Login(string badgeId);
    }
}