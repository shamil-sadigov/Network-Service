using IPWebService.Persistence;
using IPWebService.Services;
using IPWebService.Test.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace IPWebService.Test
{
    public sealed class ConnectionStringBuilderTest:IClassFixture<WebAppTestFactory>
    {
        private readonly WebAppTestFactory factory;
        private readonly IConnectionStringBuilder connStringBuilder;

        public ConnectionStringBuilderTest(WebAppTestFactory factory)
        {
            connStringBuilder = factory.Services.GetService<IConnectionStringBuilder>();
        }


        [Fact(DisplayName = "IConnectionStringBuilder is registered")]
        public void IConnectionStringBuilderIsRegistered()
        {
            Assert.True(connStringBuilder != null, "IConnectionStringBuilder should be registered");
        }


        [Fact(DisplayName = "IConnectionStringBuilder.Build(params ...) => with relevant params returns valid connectionString")]
        public void IConnectionStringBuilderBuildsValidString()
        {
            var dbOps = new AppDbOptions()
            {
                Host = "192.168.18.10",
                Password = "Creedence102938",
                Port = 5321,
                UserName = "guest"
            };

            string dbName = "testDb";

            string connString = connStringBuilder.Build(dbOps, dbName);

            Assert.True(connString != null, "Connection string should not be null");
            Assert.True(connString.Contains(dbName), "Connection string should contain Database name");
            Assert.True(connString.Contains(dbOps.Host), "Connection string should contain Host");
            Assert.True(connString.Contains(dbOps.Password), "Connection string should contain Password");
            Assert.True(connString.Contains(dbOps.Port.ToString()), "Connection string should contain Port");
            Assert.True(connString.Contains(dbOps.UserName), "Connection string should contain UserName");
        }




        [Fact(DisplayName = "IConnectionStringBuilder.Build(params ...) => with invalid params throw relevant Exception")]
        public void IConnectionStringBuilderThrowRelevantException()
        {
            try
            {
                connStringBuilder.Build(null, null);
            }
            catch (Exception ex)
            {
                Assert.True(ex is ArgumentNullException, "ArgumentNullException should be thrown");
            }

            try
            {
                connStringBuilder.Build(new AppDbOptions(), null);
            }
            catch (Exception ex)
            {
                Assert.True(ex is ArgumentNullException, "ArgumentNullException should be thrown");
            }


            try
            {
                connStringBuilder.Build(null, "TestDb");
            }
            catch (Exception ex)
            {
                Assert.True(ex is ArgumentNullException, "ArgumentNullException should be thrown");
            }

        }

    }
}
