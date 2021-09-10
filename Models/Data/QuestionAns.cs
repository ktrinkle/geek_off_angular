using System.ComponentModel.DataAnnotations;

namespace GeekOff.Data
{
    public class QuestionAns
    {
        [Key]
        [MaxLength(6)]
        public string Yevent { get; set; }
        [Key]
        public int QuestionNo { get; set; }
        [Key]
        public int? RoundNo { get; set; }
        public string TextQuestion { get; set; }
        public string TextAnswer { get; set; }
        public bool MultipleChoice { get; set; }
        public string WrongAnswer1 { get; set; }
        public string WrongAnswer2 { get; set; }
        public string WrongAnswer3 { get; set; }
    }
}