
namespace GeekOff.Models
{
    public class AppSettings
    {
        public const string App = "AppSettings";
        public string? Secret { get; set; }
        public string? Salt { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string JWTKeyId { get; set; }
        public string GeekOMaticUser { get; set; }
    }
}
