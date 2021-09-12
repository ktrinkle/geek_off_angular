using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekOff.Data
{
    [Table("current_team")]
    public partial class CurrentTeam
    {
        [Key]
        public int Id { get; set; }
        public int RoundNo { get; set; }
        public int TeamNo { get; set; }
    }
}