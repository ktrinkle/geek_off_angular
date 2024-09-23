
namespace GeekOff.Models
{
    public class AppSettings
    {
        public const string App = "AppSettings";
        public string? Secret { get; set; }
        public string? Salt { get; set; }
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string JWTKeyId { get; set; } = string.Empty;
        public string GeekOMaticUser { get; set; } = string.Empty;
        public int HashCount { get; set; }
    }
}
