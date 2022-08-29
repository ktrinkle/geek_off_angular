using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GeekOff.Models
{
    public class JWTDto
    {
        public int? TeamNum { get; set; }
        public string? AdminName { get; set; }
        public string? UserName { get; set; }
        public Guid? SessionGuid { get; set; }
    }
}
