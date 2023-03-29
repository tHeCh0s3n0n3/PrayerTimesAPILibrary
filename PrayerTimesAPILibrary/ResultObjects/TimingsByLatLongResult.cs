namespace PrayerTimesAPILibrary.ResultObjects;

public class TimingsByLatLongResult : TimingsResultBase
{
    public string Latitude { get; set; }

    public string Longitude { get; set; }

    public TimingsByLatLongResult(string latitude, string longitude)
    {
        Latitude = latitude ?? throw new ArgumentNullException(nameof(latitude));
        Longitude = longitude ?? throw new ArgumentNullException(nameof(longitude));
    }
}
