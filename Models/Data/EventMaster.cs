using System.ComponentModel.DataAnnotations;

namespace GeekOff.Data
{
    public class EventMaster
    {
        [Key]
        [MaxLength(6)]
        public string Yevent { get; set; }
        public string EventName { get; set; }
        public bool SelEvent { get; set; }
    }
}