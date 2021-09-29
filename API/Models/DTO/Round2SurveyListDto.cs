using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace GeekOff.Models
{
    public class Round2SurveyList
    {
        public int QuestionNum { get; set; }
        public string QuestionText { get; set; }
        public List<Round2Answers> SurveyAnswers { get; set; }
    }
}