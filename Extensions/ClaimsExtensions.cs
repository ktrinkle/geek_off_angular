using System;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using GeekOff.Services;

namespace GeekOff.Helpers
{
    public class AddRolesClaimsTransformation : IClaimsTransformation
    {
        private readonly ILoginService _loginService;
    
        public AddRolesClaimsTransformation(ILoginService loginService)
        {
            _loginService = loginService;
        }
    
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            // Clone current identity
            var clone = principal.Clone();
            var newIdentity = (ClaimsIdentity)clone.Identity;
    
            // Support AD and local accounts
            var nameId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier ||
                                                            c.Type == ClaimTypes.Name);
            if (nameId == null)
            {
                return principal;
            }
    
            // Get user from database
            var user = await _loginService.Login(nameId.Value);
            if (user == null)
            {
                return principal;
            }
    
            // Add role claims to cloned identity
            foreach(var roleName in user.Roles)
            {
                var claim = new Claim(newIdentity.RoleClaimType, roleName);
                newIdentity.AddClaim(claim);
            }
    
            return clone;
        }
    }
}