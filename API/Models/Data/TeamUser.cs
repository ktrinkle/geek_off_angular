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
        public string Yevent { get; set; }
        public int TeamNo { get; set; }
        public string BadgeId { get; set; }
        public string Username { get; set; }
        public string PlayerName { get; set; }
        public string WorkgroupName { get; set; }
        public int? PlayerNum { get; set; }
        public bool AdminFlag { get; set; }
        public DateTime LoginTime { get; set; }
   }
}