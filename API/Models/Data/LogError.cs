using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekOff.Data
{
    [Table("log_error")]
    public partial class LogError
    {
        [Key]
        public int Id { get; set; }
        public string ErrorMessage { get; set; }
    }
}
