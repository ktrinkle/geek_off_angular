using System;
using System.Collections.Generic;
using System.Linq;
namespace GeekOff.Services
{
    public interface ILoginService
    {
        Task<int> GetSessionIdAsync(Guid? teamGuid);
        Task<string?> GetAdminUserAsync(string userName);
        Task<bool> GetGeekOMaticUserAsync(string token);
        Task<string> GenerateTokenAsync(LoginTokenRequest loginTokenRequest);
    }
}
