using MaxMind.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPWebService.Models.DTO
{
    public class GeoliteDTO
    {
        public City City { get; set; }
        public Country Country { get; set; }
        public Location Location { get; set; }

        [Constructor]
        public GeoliteDTO([Parameter("city")] Dictionary<string, object> city,
                        [Parameter("country")] Dictionary<string, object> country,
                        [Parameter("location")] Dictionary<string, object> location)
        {
            City = city ?? default(City);
            Country = country ?? default(Country);
            Location = location ?? default(Location);
        }
    }
}
