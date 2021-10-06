using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace GeekOff.Models
{
    public class Round1Scores
    {
        public int TeamNum { get; set; }
        public string TeamName { get; set; }
        public List<Round1ScoreDetail> Q { get; set; }
        public int? TeamScore { get; set; }
        public int? Bonus { get; set; }
        public int? Rnk { get; set; }
        
    }

    public class Round1ScoreDetail
    {
        public int QuestionId { get; set; }
        public int? QuestionScore { get; set; }
    }
}