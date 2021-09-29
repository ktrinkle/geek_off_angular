using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekOff.Data
{
    [Table("question_ans")]
    public partial class QuestionAns
    {
        [MaxLength(6)]
        public string Yevent { get; set; }
        public int QuestionNo { get; set; }
        public int RoundNo { get; set; }
        public string TextQuestion { get; set; }
        public string TextAnswer { get; set; }
        public bool? MultipleChoice { get; set; }
        public string TextAnswer2 { get; set; }
        public string TextAnswer3 { get; set; }
        public string TextAnswer4 { get; set; }
        public string CorrectAnswer { get; set; }
    }
}