using System;
using System.ComponentModel.DataAnnotations;

namespace GeekOff.Data
{
    public partial class FundraisingTotal
    {
        [Key]
        [MaxLength(6)]
        public string Yevent { get; set; }
        public decimal? Totaldollar { get; set; }
    }
}