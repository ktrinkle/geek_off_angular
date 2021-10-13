using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekOff.Data
{
    [Table("roundresult")]
    public partial class Roundresult
    {
        [MaxLength(6)]
        public string Yevent { get; set; }
        public int RoundNo { get; set; }
        public int TeamNo { get; set; }
        public decimal? Ptswithbonus { get; set; }
        public int? Rnk { get; set; }
    }
}
