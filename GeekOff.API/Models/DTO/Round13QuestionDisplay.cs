namespace GeekOff.Models;

public class Round13QuestionDisplay
{
    public int QuestionNum { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public List<Round1Answers> Answers { get; set; } = [];
    public string CorrectAnswer { get; set; }  = string.Empty;
    public QuestionAnswerType AnswerType { get; set; }
    public string? MediaFile  { get; set; }
    public string? MediaType { get; set; }
}

