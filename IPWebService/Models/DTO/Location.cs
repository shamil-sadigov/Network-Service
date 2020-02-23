using NpgsqlTypes;
using System.Collections.Generic;
using static IPWebService.Helpers.Guard;
using static IPWebService.Models.GeoliteKeywords;


namespace IPWebService.Models.DTO
{
    public sealed class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string TimeZone { get; set; }

        public static implicit operator Location(Dictionary<string, object> locationKeyValues)
        {
            if (locationKeyValues is null)
                NullArgument.Throw(argument: nameof(locationKeyValues));

            var location = new Location();

            location.Latitude = locationKeyValues.TryGetValue(Lat, out object latitude)
                                ? (double)latitude
                                : KeyNotFound.Throw<double>(Lat);

            location.Longitude = locationKeyValues.TryGetValue(Long, out object longitude)
                                 ? (double)longitude
                                 : KeyNotFound.Throw<double>(Long);

            location.TimeZone = locationKeyValues.TryGetValue(Time_Zone, out object timeZone)
                                ? (string)timeZone
                                : KeyNotFound.Throw<string>(Time_Zone);

            return location;
        }


        public static implicit operator NpgsqlPoint(Location location)
         => new NpgsqlPoint(x: location.Longitude, y: location.Latitude);

    }
}
