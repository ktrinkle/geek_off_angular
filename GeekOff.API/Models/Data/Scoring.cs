using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekOff.Data
{
    [Table("scoring")]
    public class Scoring
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(6)]
        public string Yevent { get; set; } = string.Empty;
        public int TeamNum { get; set; }
        public int RoundNum { get; set; }
        public int QuestionNum { get; set; }
        public string? TeamAnswer { get; set; }
        public int? PlayerNum { get; set; }
        public int? PointAmt { get; set; }
        public int? Round3neg { get; set; }
        public int? FinalJep { get; set; }
        public DateTime Updatetime { get; set; }
    }
}
