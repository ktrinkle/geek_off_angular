using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekOff.Data
{
    [Table("team_reference")]
    public class Teamreference
    {
        [MaxLength(6)]
        public string Yevent { get; set; } = string.Empty;
        public int TeamNum { get; set; }
        public string? Teamname { get; set; }
        public decimal? Dollarraised { get; set; }
        public Guid TeamGuid { get; set; }
        public DateTime LoginTime { get; set; }
    }
}
