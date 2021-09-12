using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekOff.Data
{
    [Table("round1score")]
    public partial class Round1score
    {
        [Key]
        [MaxLength(6)]
        public string Yevent { get; set; }
        public int? RowName { get; set; }
        public string RespTxt { get; set; }
    }
}