using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GeekOff.Models
{
    public class CurrentQuestionDto
    {
        public int QuestionNum { get; set; }
        public int Status { get; set; }
    }
}
