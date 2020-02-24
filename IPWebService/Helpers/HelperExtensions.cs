using IPWebService.Models.DTO;
using IPWebService.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using static MaxMind.Db.Reader;

namespace IPWebService.Helpers
{
    public static class HelperExtensions
    {
        public static IEnumerable<GeoObject> MapToGeoObjects(this IEnumerable<ReaderIteratorNode<GeoliteDTO>> dtoList)
        {
            foreach (var dto in dtoList)
                yield return new GeoObject()
                {
                    City = dto.Data.City?.Name,
                    Country = dto.Data.Country?.Name,
                    IPAddress = dto.Start,
                    TimeZone = dto.Data.Location?.TimeZone,
                    UpdateTime = DateTime.Now,
                    Point = dto.Data.Location == null
                            ? null
                            : new NpgsqlPoint?(dto.Data.Location),
                };
        }



        public static void SetConnectionString(this IConfiguration configuration, string newconnectionString)
        {
            string connStringSection = "ConnectionString";

            string settingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

            var appsettingsJson = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(settingsPath));

            appsettingsJson[connStringSection] = newconnectionString;

            File.WriteAllText(settingsPath, JsonConvert.SerializeObject(appsettingsJson, Formatting.Indented));
        }


        public static string GetConnectionString(this IConfiguration configuration)
        {
            return configuration.GetSection("ConnectionString").Value;
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            var assemblyName = string.Concat(Assembly.GetExecutingAssembly().GetName().Name, ".xml");
            var pathToAssembly = Path.Combine(AppContext.BaseDirectory, assemblyName);

            services.AddSwaggerGen(ops =>
            {
                ops.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Schoolman - WebAPI",
                    Version = "v1"

                });
                ops.IncludeXmlComments(pathToAssembly);
            

            });
        }
    }
}
