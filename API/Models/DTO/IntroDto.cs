using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GeekOff.Models
{
    public class IntroDto
    {
        public int TeamNum { get; set; }
        public string TeamName { get; set; }
        public string Member1 { get; set; }
        public string Member2 { get; set; }
        public string Workgroup1 { get; set; }
        public string Workgroup2 { get; set; }
    }
}
