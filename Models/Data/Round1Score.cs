using System.ComponentModel.DataAnnotations;

namespace GeekOff.Data
{
    public class Round1score
    {
        [Key]
        [MaxLength(6)]
        public string Yevent { get; set; }
        public int? RowName { get; set; }
        [Key]
        public string RespTxt { get; set; }
    }
}