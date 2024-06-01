using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GeekOff.Models
{
    public class NewTeamEntry
    {
        public bool SuccessInd { get; set; }
        public int? TeamNum { get; set; }
        public Guid TeamGuid { get; set; }
        public string TeamName { get; set; }
    }
}
