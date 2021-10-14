using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekOff.Data
{
    [Table("event_master")]
    public partial class EventMaster
    {
        [Key]
        [MaxLength(6)]
        public string Yevent { get; set; }
        public string EventName { get; set; }
        public bool? SelEvent { get; set; }
    }
}
