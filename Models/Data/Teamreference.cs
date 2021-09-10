using System;
using System.ComponentModel.DataAnnotations;

namespace GeekOff.Data
{
    public class Teamreference
    {
        [Key]
        [MaxLength(6)]
        public string Yevent { get; set; }
        [Key]
        public int TeamNo { get; set; }
        public string Teamname { get; set; }
        public string Member1 { get; set; }
        public string Member2 { get; set; }
        public decimal? Dollarraised { get; set;}
        public string Workgroup1 { get; set; }
        public string Workgroup2 { get; set; }
    }
}