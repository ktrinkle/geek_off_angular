using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GeekOff.Models
{
    public class Round1QuestionDto
    {
        public int QuestionNum { get; set; }
        public string QuestionText { get; set; }  = string.Empty;
        public List<Round1Answers> Answers { get; set; } = [];
        public DateTime ExpireTime { get; set; }
        public QuestionAnswerType AnswerType { get; set; }

    }

    public class Round1Answers
    {
        public int AnswerId { get; set; }
        public string Answer { get; set; }  = string.Empty;
    }
    public enum QuestionAnswerType { MultipleChoice, Match, FreeText }
}
