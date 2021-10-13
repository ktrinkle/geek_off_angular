using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GeekOff.Models
{
    public class Round23Scores
    {
        public int TeamNo { get; set; }
        public string TeamName { get; set; }
        public int? TeamScore { get; set; }
        public int? Rnk { get; set; }
    }
}
