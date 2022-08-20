using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GeekOff.Models
{
    public class ApiResponse
    {
        public bool SuccessInd { get; set; }
        public string? Response { get; set; }
    }
}
