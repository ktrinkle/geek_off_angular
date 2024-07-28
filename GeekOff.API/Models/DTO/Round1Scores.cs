namespace GeekOff.Models;

public class Round1Scores
{
    public int TeamNum { get; set; }
    public string TeamName { get; set; }  = string.Empty;
    public List<Round1ScoreDetail> Q { get; set; } = [];
    public int? TeamScore { get; set; }
    public int? Bonus { get; set; }
    public int? Rnk { get; set; }

}

public class Round1ScoreDetail
{
    public int QuestionNum { get; set; }
    public int? QuestionScore { get; set; }
}
