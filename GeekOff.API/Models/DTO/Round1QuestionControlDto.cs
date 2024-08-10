namespace GeekOff.Models;

public class Round1QuestionControlDto
{
    public int QuestionNum { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public QuestionAnswerType AnswerType { get; set; }
    public string AnswerText { get; set; } = string.Empty;
}


