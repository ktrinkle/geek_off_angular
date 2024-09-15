namespace GeekOff.Models
{
    public class JWTDto
    {
        public int? TeamNum { get; set; }
        public string? AdminName { get; set; }
        public string? UserName { get; set; }
        public Guid? SessionGuid { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
