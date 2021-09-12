using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekOff.Data
{
    [Table("team_reference")]
    public class Teamreference
    {
        [MaxLength(6)]
        public string Yevent { get; set; }
        public int TeamNo { get; set; }
        public string Teamname { get; set; }
        public string Member1 { get; set; }
        public string Member2 { get; set; }
        public decimal? Dollarraised { get; set;}
        public string Workgroup1 { get; set; }
        public string Workgroup2 { get; set; }
    }
}