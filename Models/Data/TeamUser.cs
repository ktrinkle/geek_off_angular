using System;
using System.ComponentModel.DataAnnotations;

namespace GeekOff.Data
{
    public class TeamUser
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(6)]
        public string Yevent { get; set; }
        public int TeamNo { get; set; }
        public string BadgeId { get; set; }
        public string Username { get; set; }
        public bool AdminFlag { get; set; }
        public DateTime LoginTime { get; set; }
   }
}