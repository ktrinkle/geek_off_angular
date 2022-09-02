using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GeekOff.Models;

namespace GeekOff.Services
{
    public interface ILoginService
    {
        Task<BearerDto> PlayerLoginAsync(string yEvent, Guid teamGuid);
        Task<BearerDto> AdminLoginAsync(string userName);
        Task<BearerDto> GeekOMaticLoginAsync(string token);
        Task<int> GetSessionIdAsync(Guid? teamGuid);
        Task<string?> GetAdminUserAsync(string userName);
        Task<bool?> GetGeekOMaticUserAsync(string token);
    }
}
