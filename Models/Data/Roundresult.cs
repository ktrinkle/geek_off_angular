using System;
using System.ComponentModel.DataAnnotations;

namespace GeekOff.Data
{
    public class Roundresult
    {
        [Key]
        [MaxLength(6)]
        public string Yevent { get; set; }
        [Key]
        public int RoundNo { get; set; }
        [Key]
        public int TeamNo { get; set; }
        public decimal Ptswithbonus { get; set; }
        public int? rnk { get; set; }
    }
}