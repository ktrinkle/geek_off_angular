using System.ComponentModel.DataAnnotations;

namespace GeekOff.Data
{
    public partial class CurrentTeam
    {
        [Key]
        public int Id { get; set; }
        public int RoundNo { get; set; }
        public int TeamNo { get; set; }
    }
}