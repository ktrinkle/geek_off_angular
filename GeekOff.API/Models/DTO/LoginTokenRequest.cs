namespace GeekOff.Models
{
    public class LoginTokenRequest
    {
        public Guid SessionGuid { get; set; }
        public bool AdminFlag { get; set; }
        public int TeamNum { get; set; }
        public string? AdminUsername { get; set; }
    }
}
