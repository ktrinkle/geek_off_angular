using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekOff.Data
{
    [Table("scoring")]
    public class Scoring
    {
        [MaxLength(6)]
        public string Yevent { get; set; }
        public int TeamNo { get; set; }
        public int RoundNo { get; set; }
        public int QuestionNo { get; set; }
        public string TeamAnswer { get; set; }
        public int? PlayerNum { get; set; }
        public int? PointAmt { get; set; }
        public int? Round3neg { get; set;}
        public int? FinalJep { get; set; }
        public DateTime Updatetime { get; set; }
    }
}