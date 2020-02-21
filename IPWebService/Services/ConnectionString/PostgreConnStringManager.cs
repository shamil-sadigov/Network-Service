using IPWebService.Persistence;
using Microsoft.Extensions.Configuration;
using System;

namespace IPWebService.Services
{
    public sealed class PostgreConnStringManager : IConnectionStringManager
    {
        private readonly IConfiguration configuration;

        public PostgreConnStringManager(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string CurrentConnectionString =>
            configuration.GetConnectionString("Postgre");

        public string NewConnectionString(IConnectionStringBuilder connectionBuilder)
        {
            var options = configuration.GetSection("DbOptions:Postgre").Get<AppDbOptions>();
            return connectionBuilder.Build(options, dbName: $"Geolite_{DateTime.Now}");
        }

        public void SetConnectionString(string connectionString)
            => configuration["ConnectionStrings:Postgre"] = connectionString;
        
    }
}
