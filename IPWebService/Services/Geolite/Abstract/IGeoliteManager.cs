using MaxMind.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPWebService.Services.Geolite
{
    public interface IGeoliteManager
    {
        Task<string> InstallDbFile(string directoryPath);
        Task MigrateToDbContext(string geoliteDbFile, DbContext dbContext);
    }


    public class GeoliteManager : IGeoliteManager
    {

        private readonly IGeoliteHttpClient httpClient;
        private readonly IGeoliteFileService fileService;

        public GeoliteManager(IGeoliteHttpClient geoliteHttpClient, IGeoliteFileService geoliteFileService)
        {
            this.httpClient = geoliteHttpClient;
            this.fileService = geoliteFileService;
        }

        public async Task<string> InstallDbFile(string directoryPath)
        {
            string databaseFileGzip = await httpClient.PullGeoliteDataBaseAsync(directoryPath);
            string databaseArchive = fileService.DecompressGzip(databaseFileGzip);
            string dbFile = fileService.ExtractDbFileFromArchive(databaseArchive);

            return dbFile;
        }

        public async Task MigrateToDbContext(string geoliteDbFile, DbContext dbContext)
        {
            if (!await dbContext.Database.CanConnectAsync())
                throw new Exception("Cannot connect to database");

            dbContext.Database.EnsureCreated();






        }
    }
}
