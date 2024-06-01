using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GeekOff.Models
{
    public class Round2AnswerDto
    {
        public string YEvent { get; set; }
        public int QuestionNum { get; set; }
        public int PlayerNum { get; set; }
        public int TeamNum { get; set; }
        public string Answer { get; set; }
        public int Score { get; set; }
        public int AnswerNum { get; set; }

    }
}
