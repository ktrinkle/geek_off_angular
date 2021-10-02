using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace GeekOff.Data
{
    [Table("current_question")]
    public partial class CurrentQuestion
    {
        [Key]
        public int Id { get; set; }
        public string yEvent { get; set; }
        public int QuestionNum { get; set; }
        public int Status { get; set; }
        public DateTime QuestionTime { get; set; }
    }
}