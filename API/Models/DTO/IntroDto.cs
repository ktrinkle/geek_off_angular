using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace GeekOff.Models
{
    public class IntroDto
    {        
        public int TeamNo { get; set; }
        public string Teamname { get; set; }
        public string Member1 { get; set; }
        public string Member2 { get; set; }
        public string Workgroup1 { get; set; }
        public string Workgroup2 { get; set; }
    }
}