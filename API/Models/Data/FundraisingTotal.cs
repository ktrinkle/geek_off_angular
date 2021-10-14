using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekOff.Data
{
    [Table("fundraising_total")]
    public partial class FundraisingTotal
    {
        [Key]
        [MaxLength(6)]
        public string Yevent { get; set; }
        public decimal? Totaldollar { get; set; }
    }
}
