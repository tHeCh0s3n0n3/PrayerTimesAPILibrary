namespace PrayerTimesAPILibrary.ResultObjects;

public class TimingsResultBase
{
    public DateTime? GregorianDate { get; set; }

    public string? HijriDate { get; set; }

    public string? Imsak { get; set; }
    public string? Fajr { get; set; }
    public string? Sunrise { get; set; }
    public string? Duhr { get; set; }
    public string? Asr { get; set; }
    public string? Sunset { get; set; }
    public string? Maghrib { get; set; }
    public string? Isha { get; set; }
    public string? Midnight { get; set; }
}
