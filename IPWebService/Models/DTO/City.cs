using System.Collections.Generic;
using static IPWebService.Helpers.Guard;
using static IPWebService.Models.GeoliteKeywords;

namespace IPWebService.Models.DTO
{
    public sealed class City
    {
        /// <summary>
        /// City name in english
        /// </summary>
        public string Name { get; set; }

        public static implicit operator City(Dictionary<string, object> cityKeyValues)
        {
            if (cityKeyValues is null)
                NullArgument.Throw(argument: nameof(cityKeyValues));

            var city = new City();

            if (cityKeyValues.TryGetValue(Names, out object cityNames))
            {
                var cityNameKeyValues = (IDictionary<string, object>)cityNames;

                city.Name = cityNameKeyValues.TryGetValue(En, out object cityName)
                                ? (string)cityName
                                : KeyNotFound.Throw<string>(key: "cityName");
            }
            else
                KeyNotFound.Throw(key: Names);

            return city;
        }
    }
}
