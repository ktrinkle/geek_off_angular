using System.Linq;
using System.Security.Claims;

namespace GeekOff.Helpers
{
    public static class ClaimsPrincipalExtension
    {
        public static string UserId(this ClaimsPrincipal principal)
        {
            var claim = principal.Claims.FirstOrDefault(c => c.Type == "preferred_username");
            try
            {
                return claim.Value;
            }
            catch
            {
                return null;
            }
        }
    }
}
