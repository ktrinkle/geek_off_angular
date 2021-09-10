using System;
using System.ComponentModel.DataAnnotations;

namespace GeekOff.Data
{
    public class UserAnswer
    {
        [Key]
        [MaxLength(6)]
        public string Yevent { get; set; }
        [Key]
        public int TeamNo { get; set; }
        [Key]
        public int QuestionNo { get; set; }
        [Key]
        public int? RoundNo { get; set; }
        public string TextAnswer { get; set; }
        public string AnswerUser { get; set; }
        public DateTime AnswerTime { get; set; }
    }
}