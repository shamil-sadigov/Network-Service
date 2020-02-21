using IPWebService.Persistence;
using IPWebService.Services;
using IPWebService.Test.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IPWebService.Test
{
    public sealed class GeoliteClientTest : IClassFixture<WebAppTestFactory>
    {
        private readonly WebAppTestFactory factory;
        private readonly IGeoliteHttpClient geoliteClient;
        private readonly IWebHostEnvironment hostEnvironment;

        public GeoliteClientTest(WebAppTestFactory factory)
        {
            geoliteClient = factory.Services.GetService<IGeoliteHttpClient>();
            hostEnvironment = factory.Services.GetService<IWebHostEnvironment>();
            this.factory = factory;
        }


        [Fact(DisplayName = "IGeoliteClient is registered")]
        public void IGeoliteClientIsRegistered()
        {
            Assert.True(geoliteClient != null, "IGeoliteClient should be registered");
        }


        [Fact(DisplayName = "IGeoliteClient.PullGeoliteDatabase => Pulls Geolite database in test directory")]
        public async Task IGeoliteClientPullsGeoliteDatabaseInTestDirectory()
        {
            var root = hostEnvironment.ContentRootPath;
            var testDirectory = Path.Combine(root, "GeoliteTest");

            // will create if does not exists & won't create if exist
            Directory.CreateDirectory(testDirectory);
            
            string geoliteDbpath = await geoliteClient.PullGeoliteDataBaseArchive(testDirectory);

            Assert.True(geoliteDbpath!=null, "Geolite db file path should not be null");
            Assert.True(geoliteDbpath.Contains(testDirectory), "DB file path should be in our specified destination directory");
            Assert.True(File.Exists(geoliteDbpath), "Geolite DB should exist");

            // I decided to delete testDirectory
            // But, if you want to check Database file in testDirectory, just comment below line
            Directory.Delete(testDirectory, true);
        }




        [Fact(DisplayName = "IGeoliteClient.PullGeoliteDatabase => Throw exception if destination parameters is invalid")]
        public async Task IGeoliteClientThrowExceptionWhenPullingDatabaseIfDestinationParameterIsInvalid()
        {
            try
            {
                string geoliteDbpath = await geoliteClient.PullGeoliteDataBaseArchive(null);
            }
            catch (Exception ex)
            {
                Assert.True(ex is ArgumentNullException, "Should throw ArgumentNullException");
            }

            try
            {
                string geoliteDbpath = await geoliteClient.PullGeoliteDataBaseArchive("...");
            }
            catch (Exception ex)
            {
                Assert.True(ex is DirectoryNotFoundException, "Should throw DirectoryNotFoundException");
            }
        }
    }
}
