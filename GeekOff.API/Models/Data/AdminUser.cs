using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekOff.Data
{
    [Table("admin_user")]
    public class AdminUser
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(6)]
        public string? Username { get; set; }
        public string? AdminName { get; set; }
        public string? Password { get; set; }
        public DateTime LoginTime { get; set; }
    }
}
