using IPWebService.Persistence;
using IPWebService.Services;
using IPWebService.Services.Geolite;
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
    public sealed class GeoliteManagerTest : IClassFixture<WebAppTestFactory>
    {
        private readonly WebAppTestFactory factory;
        private readonly IGeoliteManager geoliteManager;
        private readonly IWebHostEnvironment hostEnvironment;

        public GeoliteManagerTest(WebAppTestFactory factory)
        {
            geoliteManager = factory.Services.GetService<IGeoliteManager>();
            hostEnvironment = factory.Services.GetService<IWebHostEnvironment>();
            this.factory = factory;
        }


        [Fact(DisplayName = "IGeoliteManager is registered")]
        public void IGeoliteManagerIsRegistered()
        {
            Assert.True(geoliteManager != null, "IGeoliteFileService should be registered");
        }


        [Fact(DisplayName = "IGeoliteManager Installs Db file in specified directory parameter")]
        public async Task IGeoliteManagerInstallsDbFileInDirectory()
        {
            var root = hostEnvironment.ContentRootPath;
            var testDirectory = Path.Combine(root, "GeoliteTest");

            Directory.CreateDirectory(testDirectory);

            string dbFilePath = await geoliteManager.InstallDbFileAsync(testDirectory);

            Assert.True(dbFilePath != null, "Geolite db file path should not be null");
            Assert.True(dbFilePath.Contains(testDirectory), "DB file path should be in our specified destination directory");
            Assert.True(File.Exists(dbFilePath), "Geolite DB should exist");
        }
    }
}
