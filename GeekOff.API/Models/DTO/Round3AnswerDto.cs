using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GeekOff.Models
{
    public class Round3AnswerDto
    {
        public string YEvent { get; set; }  = string.Empty;
        public int QuestionNum { get; set; }
        public int TeamNum { get; set; }
        public int? Score { get; set; }
    }
}
