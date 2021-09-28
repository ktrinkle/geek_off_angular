using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace GeekOff.Models
{
    public class UserInfoDto
    {
        public string UserName { get; set; }
        public int TeamNum { get; set; }
        public int? PlayerNum { get; set; }
        public List<string> Roles { get; set; }
    }
}