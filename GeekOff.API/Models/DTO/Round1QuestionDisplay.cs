using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GeekOff.Models
{
    public class Round1QuestionDisplay
    {
        public int QuestionNum { get; set; }
        public string QuestionText { get; set; }
        public List<Round1Answers> Answers { get; set; }
        public string CorrectAnswer { get; set; }
        public QuestionAnswerType AnswerType { get; set; }
        public string MediaFile { get; set; }
        public string MediaType { get; set; }
    }

}
