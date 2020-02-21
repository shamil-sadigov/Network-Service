using IPWebService.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using static IPWebService.Helpers.Guard;

namespace IPWebService.Services
{
    public sealed class PostgreConnStringManager : IConnectionStringManager
    {
        private readonly IConfiguration configuration;
        private readonly IConnectionStringBuilder connectionStringBuilder;

        public PostgreConnStringManager(IConfiguration configuration,
                                        IConnectionStringBuilder connectionStringBuilder)
        {
            this.configuration = configuration;
            this.connectionStringBuilder = connectionStringBuilder;
        }

        public string CurrentConnectionString =>
            configuration.GetConnectionString("Postgre");

        public string NewConnectionString(AppDbOptions ops)
        {
            if (ops.IsNull())
                NullArgument.Throw(argument: nameof(AppDbOptions));

            return connectionStringBuilder.Build(ops, $"Geolite_{DateTime.Now.Ticks}");
        }


        public void SetConnectionString(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                NullArgument.Throw(argument: nameof(AppDbOptions));

            configuration["ConnectionStrings:Postgre"] = connectionString;
        }

    }
}
