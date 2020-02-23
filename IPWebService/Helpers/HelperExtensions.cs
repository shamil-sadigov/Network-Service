using IPWebService.Models.DTO;
using IPWebService.Persistence;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MaxMind.Db.Reader;

namespace IPWebService.Helpers
{
    public static class HelperExtensions
    {
        public static IEnumerable<GeoObject> Map(this IEnumerable<ReaderIteratorNode<GeoliteDTO>> dtoList)
        {
            foreach (var dto in dtoList)
                yield return new GeoObject()
                {
                    City = dto.Data.City?.Name,
                    Country = dto.Data.Country?.Name,
                    IPAddress = dto.Start,
                    TimeZone = dto.Data.Location?.TimeZone,
                    Point = dto.Data.Location == null
                            ? null
                            : new NpgsqlPoint?(dto.Data.Location),
                };
        }
    }
}
