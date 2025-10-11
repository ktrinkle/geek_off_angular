namespace GeekOff.Models;

public class ColorMatrix
{
    public int Rnk { get; set; }
    public string Color { get; set; } = string.Empty;
}

public static class TeamColorConstants
{
    public static readonly List<ColorMatrix> TeamColors =
    [
        new() { Rnk = 1, Color = "B" },
        new() { Rnk = 2, Color = "G" },
        new() { Rnk = 3, Color = "R" },
    ];
}
