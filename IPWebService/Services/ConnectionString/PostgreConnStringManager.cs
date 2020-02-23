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

        public string CurrentConnectionString
        {
            get => configuration.GetConnectionString("Postgre");
            set 
            {
                if (string.IsNullOrEmpty(value))
                    NullArgument.Throw(argument: nameof(AppDbOptions));

                configuration["ConnectionStrings:Postgre"] = value;
            }
        }
            

        public string GenerateConnectionString(AppDbOptions ops)
        {
            if (ops.IsNull())
                NullArgument.Throw(argument: nameof(AppDbOptions));

            return connectionStringBuilder.Build(ops, $"Geolite_{DateTime.Now.Ticks}");
        }
    }
}
