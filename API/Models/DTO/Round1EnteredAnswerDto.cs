using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace GeekOff.Models
{
    public class Round1EnteredAnswers
    {
        public int TeamNum { get; set; }
        public int QuestionNum { get; set; }
        public string TextAnswer { get; set; }
    }
}