using System;
using System.ComponentModel.DataAnnotations;

namespace GeekOff.Data
{
    public class Scoring
    {
        [Key]
        [MaxLength(6)]
        public string Yevent { get; set; }
        [Key]
        public int TeamNo { get; set; }
        [Key]
        public int RoundNo { get; set; }
        [Key]
        public int QuestionNo { get; set; }
        public int? PointAmt { get; set; }
        public int? Round3neg { get; set;}
        public int? FinalJep { get; set; }
        public DateTime Updatetime { get; set; }
    }
}