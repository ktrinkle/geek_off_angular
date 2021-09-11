using System.ComponentModel.DataAnnotations;

namespace GeekOff.Data
{
    public partial class QuestionSurvey
    {
        [Key]
        [MaxLength(6)]
        public string Yevent { get; set; }
        [Key]
        public int QuestionNo { get; set; }
        [Key]
        public int? RoundNo { get; set; }
        [Key]
        public int SurveyOrder { get; set; }
        public string TextAnswer { get; set; }
        public int? Ptsposs { get; set; }
    }
}