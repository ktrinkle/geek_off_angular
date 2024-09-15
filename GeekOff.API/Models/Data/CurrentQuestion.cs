using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekOff.Data
{
    [Table("current_question")]
    public partial class CurrentQuestion
    {
        [Key]
        public int Id { get; set; }
        public string YEvent { get; set; } = string.Empty;
        public int QuestionNum { get; set; }
        public int Status { get; set; }
        public DateTime QuestionTime { get; set; }
    }
}
