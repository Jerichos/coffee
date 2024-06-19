namespace POLYGONWARE.Coffee.Utilities
{
public static class Util
{
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