using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekOff.Data
{
    public partial class Scoreposs
    {
        [Key]
        [MaxLength(6)]
        public string Yevent { get; set; }
        [Key]
        public int RoundNo { get; set; }
        [Key]
        public int QuestionNo { get; set; }
        public int? Ptsposs { get; set; }
    }
}