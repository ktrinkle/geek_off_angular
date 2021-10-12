using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GeekOff.Services;
using Microsoft.AspNetCore.Authentication;

namespace GeekOff.Helpers
{
    public class AddRolesClaimsTransformation : IClaimsTransformation
    {
        private readonly ILoginService _loginService;

        public AddRolesClaimsTransformation(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal userPrincipal)
        {
            // Clone current identity
            var cloneIdentity = userPrincipal.Clone();
            var newIdentity = (ClaimsIdentity)cloneIdentity.Identity;

            // Support AD and local accounts
            var nameId = userPrincipal.Claims.FirstOrDefault(c => c.Type is ClaimTypes.NameIdentifier or
                                                            ClaimTypes.Name);
            if (nameId == null)
            {
                return userPrincipal;
            }

            // Get user from database
            var user = await _loginService.Login(nameId.Value);
            return user == null ? userPrincipal : cloneIdentity;
        }
    }
}
