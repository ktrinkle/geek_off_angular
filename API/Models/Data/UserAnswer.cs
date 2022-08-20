using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekOff.Data
{
    [Table("user_answer")]
    public class UserAnswer
    {
        [MaxLength(6)]
        public string Yevent { get; set; }
        public int TeamNum { get; set; }
        public int QuestionNum { get; set; }
        public int? RoundNum { get; set; }
        public string? TextAnswer { get; set; }
        public string? AnswerUser { get; set; }
        public DateTime AnswerTime { get; set; }
    }
}
