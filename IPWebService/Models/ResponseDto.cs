using IPWebService.Persistence;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IPWebService.Models
{
    public class ResponseDto
    {
        public string IPAddress { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string TimeZone { get; set; }

        public static explicit operator ResponseDto(GeoObject geoObject)
           => new ResponseDto()
                {
                    IPAddress = geoObject.IPAddress.ToString(),
                    Longitude = geoObject.Point.HasValue ? geoObject.Point.Value.X.ToString() : null,
                    Latitude = geoObject.Point.HasValue ? geoObject.Point.Value.Y.ToString() : null,
                    City = geoObject.City,
                    Country = geoObject?.Country,
                    TimeZone = geoObject.TimeZone
                };
        
    }
}
