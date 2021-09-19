using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace GeekOff.Models
{
    public class Round1QuestionDto
    {
        public int QuestionNo { get; set; }
        public string QuestionText { get; set; }
        public List<Round1Answers> Answers { get; set; }
        public DateTime ExpireTime { get; set; }
        public QuestionAnswerType AnswerType { get; set; }
        
    }

    public class Round1Answers
    {
        public int AnswerId { get; set; }
        public string Answer { get; set; }
    }
    public enum QuestionAnswerType { MultipleChoice, Match, FreeText }
}