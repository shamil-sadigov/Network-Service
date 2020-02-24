using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IPWebService.Persistence
{
    public class GeoObject
    {
        public long Id { get; set; }
        public IPAddress IPAddress { get; set; }
        public NpgsqlPoint? Point { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string TimeZone { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
