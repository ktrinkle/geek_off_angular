using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GeekOff.Models
{
    public class Round1QuestionControlDto
    {
        public int QuestionNum { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public QuestionAnswerType AnswerType { get; set; }
        public string AnswerText { get; set; } = string.Empty;
    }

}
