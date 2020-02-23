using System.Collections.Generic;
using static IPWebService.Helpers.Guard;
using static IPWebService.Models.GeoliteKeywords;

namespace IPWebService.Models.DTO
{
    public sealed class Country
    {
        public string Name { get; set; }

        public static implicit operator Country(Dictionary<string, object> countryKeyValues)
        {
            if (countryKeyValues is null)
                NullArgument.Throw(argument: nameof(countryKeyValues));

            var country = new Country();

            if (countryKeyValues.TryGetValue(Names, out object countryNames))
            {
                var countryNameKeyValues = (IDictionary<string, object>)countryNames;

                country.Name = countryNameKeyValues.TryGetValue(En, out object countryName)
                               ? (string)countryName
                               : KeyNotFound.Throw<string>(key: "countryName");
            }
            else
                KeyNotFound.Throw(key: Names);

            return country;
        }
    }
}
