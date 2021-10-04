using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace GeekOff.Models
{
    public class Round1EnteredAnswers
    {
        public string Yevent { get; set; }
        public int TeamNum { get; set; }
        public int QuestionNum { get; set; }
        public string TextAnswer { get; set; }
    }
}