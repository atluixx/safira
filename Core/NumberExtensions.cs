namespace Safira.Core;

public static class NumberExtensions
{
    private static readonly List<(string Suffix, double Multiplier)> _formatters =
    [
        ("t", 1_000_000_000_000),
        ("b", 1_000_000_000),
        ("m", 1_000_000),
        ("k", 1_000),
    ];

    public static void AddFormatter(string suffix, double multiplier)
    {
        _formatters.Add((suffix, multiplier));
        _formatters.Sort((a, b) => b.Multiplier.CompareTo(a.Multiplier));
    }

    public static string ToCompact(this double value)
    {
        foreach (var (suffix, multiplier) in _formatters)
        {
            if (value >= multiplier)
                return $"{value / multiplier:0.##}{suffix.ToUpper()}";
        }

        return $"{value:0.##}";
    }

    public static double FromCompact(this string value)
    {
        value = value.Trim();

        foreach (var (suffix, multiplier) in _formatters)
        {
            if (value.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
            {
                var number = value[..^suffix.Length];
                return double.Parse(number) * multiplier;
            }
        }

        return double.Parse(value);
    }

    public static string ToCompact(this decimal value) => ((double)value).ToCompact();
    public static string ToCompact(this long value) => ((double)value).ToCompact();
    public static string ToCompact(this int value) => ((double)value).ToCompact();
}
