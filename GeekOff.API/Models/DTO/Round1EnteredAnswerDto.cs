namespace GeekOff.Models
{
    public class Round1EnteredAnswers
    {
        public string Yevent { get; set; } = string.Empty;
        public int TeamNum { get; set; }
        public int QuestionNum { get; set; }
        public string TextAnswer { get; set; } = string.Empty;
        public bool? AnswerStatus { get; set; }
    }
}
