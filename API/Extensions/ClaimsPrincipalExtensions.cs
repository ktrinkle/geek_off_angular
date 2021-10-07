using System.Security.Claims;
using System.Linq;

namespace GeekOff.Helpers
{
    public static class ClaimsPrincipalExtension
    {
        public static string UserId(this ClaimsPrincipal principal)
        {
            Claim claim = principal.Claims.FirstOrDefault(c => c.Type == "preferred_username");
            try
            {
                return claim.Value.Substring(0,6);
            }
            catch
            {
                return "000000";
            }
        }
    }
}