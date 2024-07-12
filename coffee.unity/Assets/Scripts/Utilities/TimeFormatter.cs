namespace POLYGONWARE.Coffee.Utilities
{
public static class TimeFormatter
{
    // format seconds dynamically. If seconds are less than 60, format to seconds, otherwise format to minutes and seconds as 1min 30s, if over hour then for to hours as 1h 30min 30s
    public static string FormatSeconds(ulong totalSeconds)
    {
        if (totalSeconds < 60)
        {
            return $"{totalSeconds:D2}s";
        }
        else if (totalSeconds < 3600)
        {
            var minutes = totalSeconds / 60;
            var seconds = totalSeconds % 60;
            return $"{minutes:D2}m {seconds:D2}s";
        }
        else
        {
            var hours = totalSeconds / 3600;
            var minutes = (totalSeconds % 3600) / 60;
            var seconds = totalSeconds % 60;
            return $"{hours:D2}h {minutes:D2}m {seconds:D2}s";
        }
    }
    
    public static string FormatSecondsToHHMM(ulong totalSeconds)
    {
        var hours = totalSeconds / 3600;
        var minutes = (totalSeconds % 3600) / 60;
        return $"{hours:D2}:{minutes:D2}";
    }
    
    // format seconds to DD:HH:MM
    public static string FormatSecondsToDDHHMM(ulong totalSeconds)
    {
        var days = totalSeconds / 86400;
        var hours = (totalSeconds % 86400) / 3600;
        var minutes = (totalSeconds % 3600) / 60;
        return $"{days:D2}:{hours:D2}:{minutes:D2}";
    }
}
}