using IPWebService.Persistence;
using IPWebService.Services;
using IPWebService.Test.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace IPWebService.Test
{
    public sealed class AppConfigurationTest:IClassFixture<WebAppTestFactory>
    {
        private readonly IConfiguration configuration;
        private readonly IOptionsSnapshot<AppDbOptions> appDbOptionsSnapshot;

        public AppConfigurationTest(WebAppTestFactory factory)
        {
            configuration = factory.Services.GetService<IConfiguration>();
            appDbOptionsSnapshot = factory.Services.GetService<IOptionsSnapshot<AppDbOptions>>();

        }


        [Fact(DisplayName = "AppDbOptions is configured")]
        public void AppDbOptionsShouldBeConfigured()
        {
            AppDbOptions dbOptions = appDbOptionsSnapshot.Value;
            Assert.True(!dbOptions.IsNull(), "AppDbOptions should be configured");
        }




        [Fact(DisplayName = "InMemoryCollection of IConfiguration works correctly")]
        public void InMemoryCollectionShouldWorkCorrectly()
        {

            string newConnString = "Assume I'm connectionString";
            configuration["ConnectionStrings:Postgre"] = newConnString;
            var connString = configuration.GetConnectionString("Postgre");
            Assert.True(connString == newConnString);
        }

    }
}
