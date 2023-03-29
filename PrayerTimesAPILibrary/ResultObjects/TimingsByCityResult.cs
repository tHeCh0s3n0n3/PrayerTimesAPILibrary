namespace PrayerTimesAPILibrary.ResultObjects;

public class TimingsByCityResult : TimingsResultBase
{
    public string City { get; set; }

    public string CountryCode { get; set; }

    public TimingsByCityResult(string city, string countryCode)
    {
        City = city ?? throw new ArgumentNullException(nameof(city)); ;
        CountryCode = countryCode ?? throw new ArgumentNullException(nameof(countryCode)); ;
    }
}
