using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GeekOff.Models
{
    public class BearerDto
    {
        public int? TeamNum { get; set; }
        public string? TeamName { get; set; }
        public string? UserName { get; set; }
        public string? HumanName { get; set; }
        public string? BearerToken { get; set; }
    }
}
