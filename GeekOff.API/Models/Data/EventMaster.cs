using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekOff.Data;

[Table("event_master")]
public partial class EventMaster
{
    [Key]
    [MaxLength(6)]
    public string Yevent { get; set; } = string.Empty;
    public string EventName { get; set; } = string.Empty;
    public bool? SelEvent { get; set; }
    public RoundControl Round2Control { get; set; }
    public RoundControl Round3Control { get; set; } = RoundControl.Jeopardy;
}

