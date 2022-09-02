using System.Linq;
using System.Security.Claims;

namespace GeekOff.Helpers
{
    public static class ClaimsPrincipalExtension
    {
        public static string TeamId(this ClaimsPrincipal principal)
        {
            var claim = principal.Claims.FirstOrDefault(c => c.Type == "teamnum");
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
