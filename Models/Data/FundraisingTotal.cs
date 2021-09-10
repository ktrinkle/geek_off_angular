using System;
using System.ComponentModel.DataAnnotations;

namespace GeekOff.Data
{
    public class FundraisingTotal
    {
        [Key]
        [MaxLength(6)]
        public string Yevent { get; set; }
        public decimal? Totaldollar { get; set; }
    }
}