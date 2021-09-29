using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace GeekOff.Models
{
    public class Round2BoardDto
    {
        public int TeamNo { get; set; }
        public List<Round2Answers> Player1Answers { get; set; }
        public List<Round2Answers> Player2Answers { get; set; }
        public int FinalScore { get; set; }
    }

    public class Round2Answers
    {
        public int QuestionNum { get; set; }
        public string Answer { get; set; }
        public int Score { get; set; }
    }
}