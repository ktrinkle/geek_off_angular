using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekOff.Data;

[Table("round_category")]
public partial class RoundCategory
{
    [MaxLength(6)]
    public int Id { get; set; }
    public string Yevent { get; set; } = string.Empty;
    public int RoundNum { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int SubCategoryNum { get; set; }
}

