using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekOff.Data
{
    [Table("scoreposs")]
    public partial class Scoreposs
    {
        [MaxLength(6)]
        public string Yevent { get; set; }
        public int RoundNo { get; set; }
        public int QuestionNo { get; set; }
        public int SurveyOrder { get; set; }
        public string QuestionAnswer { get; set; }
        public int? Ptsposs { get; set; }
    }
}