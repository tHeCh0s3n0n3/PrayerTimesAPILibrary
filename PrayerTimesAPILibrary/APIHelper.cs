using PrayerTimesAPILibrary.PrayerAPI;
using PrayerTimesAPILibrary.ResultObjects;
using System.Globalization;

namespace PrayerTimesAPILibrary;

public class APIHelper
{
    private static readonly PrayerAPI.Client _client = new(new HttpClient());

    public async static Task<DateOnly?> GetCurrentDateAsync(CancellationToken cancellationToken = default)
    {
        ApiResponse<CurrentDateSuccess>? response = null;
        if (TimeZoneInfo.TryConvertWindowsIdToIanaId(TimeZoneInfo.Local.StandardName, out string? ianaName)
            && !string.IsNullOrEmpty(ianaName))
        {
            response = await _client.V1CurrentDateAsync(ianaName, cancellationToken);
        }

        if (response?.Result?.Data.DateTime is not null)
        {
            return DateOnly.FromDateTime(response.Result.Data.DateTime);
        }
        return null;
    }

    public async static Task<TimeOnly?> GetCurrentTimeAsync(CancellationToken cancellationToken = default)
    {
        ApiResponse<CurrentTimeSuccess>? response = null;
        if (TimeZoneInfo.TryConvertWindowsIdToIanaId(TimeZoneInfo.Local.StandardName, out string? ianaName)
            && !string.IsNullOrEmpty(ianaName))
        {
            response = await _client.V1CurrentTimeAsync(ianaName, cancellationToken);
        }

        if (response?.Result.Data is not null)
        {
            return TimeOnly.FromTimeSpan(response.Result.Data);
        }
        return null;
    }

    public async static Task<long?> GetCurrentTimestampAsync(CancellationToken cancellationToken = default)
    {
        long? retval = null;
        if (TimeZoneInfo.TryConvertWindowsIdToIanaId(TimeZoneInfo.Local.StandardName, out string? ianaName)
            && !string.IsNullOrEmpty(ianaName))
        {
            ApiResponse<CurrentTimestampSuccess> response = await _client.V1CurrentTimestampAsync(ianaName, cancellationToken);
            if (long.TryParse(response?.Result.Data, out long convResult))
            {
                retval = convResult;
            }
        }

        return retval;
    }

    public async static Task<Methodssuccess_data?> GetPrayerMethodsAsync(CancellationToken cancellationToken = default)
    {
        var response = await _client.V1MethodsAsync(cancellationToken);
        return response?.Result?.Data;
    }

    #region Get Single Day Timings
    public async static Task<TimingsByCityResult> GetTimingsByCityAsync(string city, string countryCode, CancellationToken cancellationToken = default)
    {
        ApiResponse<Timings_response>? response
            = await _client.V1TimingsByCityGetAsync(city: city
                                                    , country: countryCode
                                                    , method: null
                                                    , shafaq: null
                                                    , tune: null
                                                    , school: null
                                                    , midnightMode: null
                                                    , timezonestring: null
                                                    , latitudeAdjustmentMethod: null
                                                    , adjustment: null
                                                    , iso8601: null
                                                    , cancellationToken: cancellationToken);

        TimingsByCityResult retval = new(city, countryCode);

        if (response?.Result?.Data?.Timings is not null
            && response?.Result?.Data?.Date?.Gregorian is not null
            && response?.Result?.Data?.Date?.Hijri?.Month?.Number is not null)
        {
            retval.GregorianDate = DateTime.ParseExact(response.Result.Data.Date.Gregorian.Date, "dd-MM-yyyy", null);
            
            HijriCalendar hc = new();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8629 // Nullable value type may be null.
            DateTime todaysHijriDate = new(year: int.Parse(response.Result.Data.Date.Hijri.Year)
                                           , month: response.Result.Data.Date.Hijri.Month.Number.Value
                                           , day: int.Parse(response.Result.Data.Date.Hijri.Day)
                                           , calendar: hc);
#pragma warning restore CS8629 // Nullable value type may be null.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            CultureInfo enAE = CultureInfo.CreateSpecificCulture("en-AE");
            enAE.DateTimeFormat.Calendar = hc;
            
            retval.HijriDate = todaysHijriDate.ToString("d MMMM yyyy", enAE);
            
            retval.Imsak = response.Result.Data.Timings.Imsak;
            retval.Fajr = response.Result.Data.Timings.Fajr;
            retval.Sunrise = response.Result.Data.Timings.Sunrise;
            retval.Duhr = response.Result.Data.Timings.Dhuhr;
            retval.Asr = response.Result.Data.Timings.Asr;
            retval.Sunset = response.Result.Data.Timings.Sunset;
            retval.Maghrib = response.Result.Data.Timings.Maghrib;
            retval.Isha = response.Result.Data.Timings.Isha;
            retval.Midnight = response.Result.Data.Timings.Midnight;
        }

        return retval;
    }

    public async static Task<TimingsByLatLongResult> GetTimingsByLatLongAsync(double latitude, double longitude, CancellationToken cancellationToken = default)
    {
        ApiResponse<Timings_response>? response
            = await _client.V1TimingsGetAsync(latitude: latitude
                                              , longitude: longitude
                                              , method: null
                                              , shafaq: null
                                              , tune: null
                                              , school: null
                                              , midnightMode: null
                                              , timezonestring: null
                                              , latitudeAdjustmentMethod: null
                                              , adjustment: null
                                              , iso8601: null
                                              , cancellationToken: cancellationToken);

        TimingsByLatLongResult retval = new($"{latitude}", $"{longitude}");

        if (response?.Result?.Data?.Timings is not null
            && response?.Result?.Data?.Date?.Gregorian is not null
            && response?.Result?.Data?.Date?.Hijri?.Month?.Number is not null)
        {
            retval.GregorianDate = DateTime.ParseExact(response.Result.Data.Date.Gregorian.Date, "dd-MM-yyyy", null);

            HijriCalendar hc = new();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8629 // Nullable value type may be null.
            DateTime todaysHijriDate = new(year: int.Parse(response.Result.Data.Date.Hijri.Year)
                                           , month: response.Result.Data.Date.Hijri.Month.Number.Value
                                           , day: int.Parse(response.Result.Data.Date.Hijri.Day)
                                           , calendar: hc);
#pragma warning restore CS8629 // Nullable value type may be null.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            CultureInfo enAE = CultureInfo.CreateSpecificCulture("en-AE");
            enAE.DateTimeFormat.Calendar = hc;

            retval.HijriDate = todaysHijriDate.ToString("d MMMM yyyy", enAE);

            retval.Imsak = response.Result.Data.Timings.Imsak;
            retval.Fajr = response.Result.Data.Timings.Fajr;
            retval.Sunrise = response.Result.Data.Timings.Sunrise;
            retval.Duhr = response.Result.Data.Timings.Dhuhr;
            retval.Asr = response.Result.Data.Timings.Asr;
            retval.Sunset = response.Result.Data.Timings.Sunset;
            retval.Maghrib = response.Result.Data.Timings.Maghrib;
            retval.Isha = response.Result.Data.Timings.Isha;
            retval.Midnight = response.Result.Data.Timings.Midnight;
        }

        return retval;
    }
    #endregion

    #region Get Calendar Timings
    public async static Task<List<TimingsByCityResult>> GetMonthlyTimingsByCityAsync(string city, string countryCode, double month, double year, CancellationToken cancellationToken = default)
    {
        ApiResponse<Calendar_response>? response
            = await _client.V1CalendarByCityAsync(city: city
                                                  , country: countryCode
                                                  , month: month
                                                  , year: year
                                                  , annual: false
                                                  , method: null
                                                  , shafaq: null
                                                  , tune: null
                                                  , school: null
                                                  , midnightMode: null
                                                  , latitudeAdjustmentMethod: null
                                                  , adjustment: null
                                                  , iso8601: null
                                                  , cancellationToken: cancellationToken);

        List<TimingsByCityResult> retval = new();

        if (response?.Result?.Data is not null
            && response.Result.Data.Any())
        {
            foreach (Result_item result in response.Result.Data)
            {
                if (result.Date?.Gregorian is not null
                    && result.Date?.Hijri?.Month?.Number is not null
                    && result.Timings is not null)
                {
                    TimingsByCityResult newTiming = new(city, countryCode)
                    {
                        GregorianDate = DateTime.ParseExact(result.Date.Gregorian.Date, "dd-MM-yyyy", null)
                    };
                    HijriCalendar hc = new();
                    DateTime todaysHijriDate = new(year: int.Parse(result.Date.Hijri.Year)
                                                   , month: result.Date.Hijri.Month.Number.Value
                                                   , day: int.Parse(result.Date.Hijri.Day)
                                                   , calendar: hc);
                    CultureInfo enAE = CultureInfo.CreateSpecificCulture("en-AE");
                    enAE.DateTimeFormat.Calendar = hc;

                    newTiming.HijriDate = todaysHijriDate.ToString("d MMMM yyyy", enAE);

                    newTiming.Imsak = result.Timings.Imsak;
                    newTiming.Fajr = result.Timings.Fajr;
                    newTiming.Sunrise = result.Timings.Sunrise;
                    newTiming.Duhr = result.Timings.Dhuhr;
                    newTiming.Asr = result.Timings.Asr;
                    newTiming.Sunset = result.Timings.Sunset;
                    newTiming.Maghrib = result.Timings.Maghrib;
                    newTiming.Isha = result.Timings.Isha;
                    newTiming.Midnight = result.Timings.Midnight;

                    retval.Add(newTiming);
                }
            }
        }

        return retval;
    }
    #endregion
}
