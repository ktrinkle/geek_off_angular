using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekOff.Data
{
    [Table("team_user")]
    public class TeamUser
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(6)]
        public string Yevent { get; set; } = string.Empty;
        public int TeamNum { get; set; }
        public string? BadgeId { get; set; }
        public string? Username { get; set; }
        public string? PlayerName { get; set; }
        public string? WorkgroupName { get; set; }
        public int? PlayerNum { get; set; }
        public DateTime LoginTime { get; set; }
        public Guid SessionId { get; set; }
    }
}
