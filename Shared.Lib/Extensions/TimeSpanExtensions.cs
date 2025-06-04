namespace Shared.Lib.Extensions;

public static class TimeSpanExtensions
{
    public static string ToReadableString(this TimeSpan span)
    {
        int days = span.Days;
        int hours = span.Hours;
        int minutes = span.Minutes;
        int seconds = span.Seconds;

        string FormatUnit(int value, string one, string few, string many)
        {
            if (value % 100 >= 11 && value % 100 <= 14)
                return $"{value} {many}";
            switch (value % 10)
            {
                case 1: return $"{value} {one}";
                case 2:
                case 3:
                case 4: return $"{value} {few}";
                default: return $"{value} {many}";
            }
        }

        List<string> parts = new List<string>();

        if (span.Duration().Days > 0) parts.Add(FormatUnit(days, "день", "дня", "дней"));
        if (span.Duration().Hours > 0) parts.Add(FormatUnit(hours, "час", "часа", "часов"));
        if (span.Duration().Minutes > 0) parts.Add(FormatUnit(minutes, "минута", "минуты", "минут"));
        if (span.Duration().Seconds > 0) parts.Add(FormatUnit(seconds, "секунда", "секунды", "секунд"));

        return parts.Count > 0 ? string.Join(", ", parts) : "0 секунд";
    }
}