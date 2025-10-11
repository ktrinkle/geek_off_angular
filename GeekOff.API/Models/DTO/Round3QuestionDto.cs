using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GeekOff.Models
{
    public class Round3QuestionDto
    {
        public int QuestionNum { get; set; }
        public decimal SortOrder { get; set; }
        public int? Score { get; set; }
        public bool Disabled { get; set; } = false;
    }

}
