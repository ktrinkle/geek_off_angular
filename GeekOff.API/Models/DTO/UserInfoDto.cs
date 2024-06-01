using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GeekOff.Models
{
    public class UserInfoDto
    {
        public string PlayerName { get; set; }
        public string UserName { get; set; }
        public int TeamNum { get; set; }
        public int? PlayerNum { get; set; }
    }
}
