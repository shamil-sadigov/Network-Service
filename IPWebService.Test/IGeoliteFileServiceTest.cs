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
    public sealed class GeoliteFileServiceTest : IClassFixture<WebAppTestFactory>
    {
        private readonly WebAppTestFactory factory;
        private readonly IGeoliteHttpClient geoliteClient;
        private readonly IGeoliteFileService fileService;
        private readonly IWebHostEnvironment hostEnvironment;

        public GeoliteFileServiceTest(WebAppTestFactory factory)
        {
            geoliteClient = factory.Services.GetService<IGeoliteHttpClient>();
            fileService = factory.Services.GetService<IGeoliteFileService>();
            hostEnvironment = factory.Services.GetService<IWebHostEnvironment>();
            this.factory = factory;
        }


        [Fact(DisplayName = "IGeoliteFileService is registered")]
        public void IGeoliteFileServiceIsRegistered()
        {
            Assert.True(fileService != null, "IGeoliteFileService should be registered");
        }


        [Fact(DisplayName = "IGeoliteFileService DecompressGzip file and Extracts Db File from Archive")]
        public async Task IGeoliteFileServiceDecompressandExtractsDbFile()
        {
            #region Arrange gzip database

            string gzipDatabase = await GetTestDbGzipFileAsync(directoryName: "GeoliteTest");
            Assert.True(gzipDatabase != null, "Gzip database should not be null");

            #endregion

            string databaseArchive = fileService.DecompressGzip(gzipDatabase);

            Assert.True(databaseArchive != null, "Database archive should not be null");
            Assert.True(File.Exists(databaseArchive), "Database archive should exist");


            string dbFile = fileService.ExtractDbFileFromArchive(databaseArchive);

            Assert.True(dbFile != null, "Database file should not be null");
            Assert.True(File.Exists(dbFile), "Database file should exist");

        }




        [Fact(DisplayName = "IGeoliteFileService.DecompressGzip throw Exception parameter DatabaseGzip file invalid")]
        public void IGeoliteClientThrowExceptionWhenPullingDatabaseIfDestinationParameterIsInvalid()
        {
            try
            {
                string archiveFile = fileService.DecompressGzip(null);
            }
            catch (Exception ex)
            {
                Assert.True(ex is ArgumentNullException, "Should throw ArgumentNullException");
            }

            try
            {
                string archiveFile =  fileService.DecompressGzip("Invalid file Name");
            }
            catch (Exception ex)
            {
                Assert.True(ex is FileNotFoundException, "Should throw DirectoryNotFoundException");
            }
        }





        [Fact(DisplayName = "IGeoliteFileService.ExtractDbFileFromArchive throw Exception parameter archive file invalid")]
        public void IGeoliteClientThrowExceptionWhenPullingDatabaseIfDestinationParameterIsInvalid1()
        {
            try
            {
                string archiveFile = fileService.ExtractDbFileFromArchive(null);
            }
            catch (Exception ex)
            {
                Assert.True(ex is ArgumentNullException, "Should throw ArgumentNullException");
            }

            try
            {
                string archiveFile = fileService.ExtractDbFileFromArchive("Invalid file Name");
            }
            catch (Exception ex)
            {
                Assert.True(ex is FileNotFoundException, "Should throw DirectoryNotFoundException");
            }
        }


        private async Task<string> GetTestDbGzipFileAsync(string directoryName)
        {
            var root = hostEnvironment.ContentRootPath;
            var testDirectory = Path.Combine(root, directoryName);

            // will create if does not exists & won't create if exist
            Directory.CreateDirectory(testDirectory);

            string geoliteDbGzipPath = await geoliteClient.PullGeoliteDataBase(testDirectory);

            return geoliteDbGzipPath;
        }
    }
}
