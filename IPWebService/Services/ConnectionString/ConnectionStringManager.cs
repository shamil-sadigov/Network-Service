using IPWebService.Helpers;
using IPWebService.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using static IPWebService.Helpers.Guard;

namespace IPWebService.Services
{
    public sealed class ConnectionStringManager : IConnectionStringManager
    {
        private readonly IConfiguration configuration;
        private readonly IConnectionStringBuilder connectionStringBuilder;
        private readonly AppDbOptions dbConnectionOptions;

        public ConnectionStringManager(IConfiguration configuration,
                                        IConnectionStringBuilder connectionStringBuilder,
                                        IOptionsSnapshot<AppDbOptions> optionsSnapshot)
        {
            this.configuration = configuration;
            this.connectionStringBuilder = connectionStringBuilder;
            dbConnectionOptions = optionsSnapshot.Value;
        }

        public string ConnectionString
        {
            get => configuration.GetConnectionString();

            set 
            {
                if (string.IsNullOrEmpty(value))
                    NullArgument.Throw(argument: nameof(AppDbOptions));
                
                configuration.SetConnectionString(value);
            }
        }
            

        public string GenerateNewConnectionString()
        {
            if (dbConnectionOptions.IsNull())
                NullArgument.Throw(argument: nameof(dbConnectionOptions));

            return connectionStringBuilder.Build(dbConnectionOptions, $"Geolite_{DateTime.Now.Ticks}");
        }
    }
}
