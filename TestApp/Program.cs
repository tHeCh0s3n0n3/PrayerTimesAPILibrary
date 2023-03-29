// See https://aka.ms/new-console-template for more information

using PrayerTimesAPILibrary;
using PrayerTimesAPILibrary.ResultObjects;

Console.WriteLine("Starting tests");

//await TestCurrentDateTime();
//await TestCurrentTime();
//await TestCurrentTimestamp();

string city = "Dubai";
string countryCode = "AE";
await TestTodaysTimingsByCity(city, countryCode);
//await TestMonthTimings(city, countryCode);

double latitude = 25.0962724;
double longitude = 55.3254198;
await TestTodaysTimingsByLatLong(latitude, longitude);

async Task TestCurrentDateTime()
{
    DateOnly? currentDateTime = await APIHelper.GetCurrentDateAsync();
    Console.WriteLine($"API Current DateTime: {currentDateTime:yyyy-MM-dd}");
}

async Task TestCurrentTime()
{
    TimeOnly? currentTime = await APIHelper.GetCurrentTimeAsync();
    Console.WriteLine($"API Current Time: {currentTime:hh:mm:ss}");
}

async Task TestCurrentTimestamp()
{
    long? currentTimestamp = await APIHelper.GetCurrentTimestampAsync();
    if (currentTimestamp is not null)
    {
        DateTime currentTimestampAsDateTime = DateTime.UnixEpoch.AddSeconds(Convert.ToDouble(currentTimestamp));
        Console.WriteLine($"API Current Timestamp: {currentTimestamp} = {currentTimestampAsDateTime:u}");
    }
    else
    {
        Console.WriteLine("Failed to get current timestamp.");
    }
}

async Task TestTodaysTimingsByCity(string city, string countryCode)
{
    TimingsByCityResult todaysTimings = await APIHelper.GetTimingsByCityAsync(city, countryCode);
    Console.Write($"Prayer Timings for {todaysTimings.City}, {todaysTimings.CountryCode} on ");
    Console.Write($"{todaysTimings.GregorianDate:yyyy-MM-dd} ");
    Console.WriteLine($"({todaysTimings.HijriDate})");
    Console.WriteLine($"Imsak: {todaysTimings.Imsak}");
    Console.WriteLine($"Fajr: {todaysTimings.Fajr}");
    Console.WriteLine($"Sunrise: {todaysTimings.Sunrise}");
    Console.WriteLine($"Duhr: {todaysTimings.Duhr}");
    Console.WriteLine($"Asr: {todaysTimings.Asr}");
    Console.WriteLine($"Sunset: {todaysTimings.Sunset}");
    Console.WriteLine($"Maghrib: {todaysTimings.Maghrib}");
    Console.WriteLine($"Isha: {todaysTimings.Isha}");
    Console.WriteLine($"Midnight: {todaysTimings.Midnight}");
}

async Task TestTodaysTimingsByLatLong(double latitude, double longitude)
{
    TimingsByLatLongResult todaysTimings = await APIHelper.GetTimingsByLatLongAsync(latitude, longitude);
    Console.Write($"Prayer Timings for {todaysTimings.Latitude} {todaysTimings.Longitude} on ");
    Console.Write($"{todaysTimings.GregorianDate:yyyy-MM-dd} ");
    Console.WriteLine($"({todaysTimings.HijriDate})");
    Console.WriteLine($"Imsak: {todaysTimings.Imsak}");
    Console.WriteLine($"Fajr: {todaysTimings.Fajr}");
    Console.WriteLine($"Sunrise: {todaysTimings.Sunrise}");
    Console.WriteLine($"Duhr: {todaysTimings.Duhr}");
    Console.WriteLine($"Asr: {todaysTimings.Asr}");
    Console.WriteLine($"Sunset: {todaysTimings.Sunset}");
    Console.WriteLine($"Maghrib: {todaysTimings.Maghrib}");
    Console.WriteLine($"Isha: {todaysTimings.Isha}");
    Console.WriteLine($"Midnight: {todaysTimings.Midnight}");
}

async Task TestMonthTimings(string city, string countryCode)
{
    List<TimingsByCityResult> monthTimings
        = await APIHelper.GetMonthlyTimingsByCityAsync(city
                                                       , countryCode
                                                       , Convert.ToDouble(DateTime.Now.Month)
                                                       , Convert.ToDouble(DateTime.Now.Year));

    foreach (TimingsByCityResult timings in monthTimings)
    {
        Console.Write($"Prayer Timings for {timings.City}, {timings.CountryCode} on ");
        Console.Write($"{timings.GregorianDate:yyyy-MM-dd} ");
        Console.WriteLine($"({timings.HijriDate})");
        Console.WriteLine($"Imsak: {timings.Imsak}");
        Console.WriteLine($"Fajr: {timings.Fajr}");
        Console.WriteLine($"Sunrise: {timings.Sunrise}");
        Console.WriteLine($"Duhr: {timings.Duhr}");
        Console.WriteLine($"Asr: {timings.Asr}");
        Console.WriteLine($"Sunset: {timings.Sunset}");
        Console.WriteLine($"Maghrib:{timings.Maghrib}");
        Console.WriteLine($"Isha: {timings.Isha}");
        Console.WriteLine($"Midnight: {timings.Midnight}");
    }
}