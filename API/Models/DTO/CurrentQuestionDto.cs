using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace GeekOff.Models
{
    public class CurrentQuestionDto
    {        
        public int QuestionNum { get; set; }
        public int Status { get; set; }
    }
}