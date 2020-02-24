using IPWebService.Helpers;
using IPWebService.Models.DTO;
using IPWebService.Persistence;
using MaxMind.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static IPWebService.Helpers.Guard;

namespace IPWebService.Services.Geolite
{
    public class GeoliteManager : IGeoliteManager
    {
        private readonly IGeoliteHttpClient httpClient;
        private readonly IGeoliteFileService fileService;

        public GeoliteManager(IGeoliteHttpClient geoliteHttpClient, IGeoliteFileService geoliteFileService)
            => (httpClient, fileService) = (geoliteHttpClient, geoliteFileService);
       

        public async Task<string> DownloadDbFileAsync(string directoryPath)
        {
            string databaseFileGzip = await httpClient.PullGeoliteDataBaseAsync(directoryPath);
            string databaseArchive = fileService.DecompressGzip(databaseFileGzip);
            string dbFile = fileService.ExtractDbFileFromArchive(databaseArchive);

            return dbFile;
        }

        public async Task MigrateToDbContext(string geoliteDbFile, DbContext dbContext)
        {
            if (string.IsNullOrEmpty(geoliteDbFile))
                NullArgument.Throw(nameof(geoliteDbFile));

            if (!File.Exists(geoliteDbFile))
                throw new FileNotFoundException($"Db file not found {geoliteDbFile}");

        
            using (var reader = new Reader(geoliteDbFile))
            {
                var dtoObjects = reader.FindAll<GeoliteDTO>();

                IEnumerable<GeoObject> models = dtoObjects.MapToGeoObjects();

                using (var transaction = await dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await dbContext.BulkInsertAsync(models.Take(10000), ops => ops.BatchSize = 5000);
                        await transaction.CommitAsync();
                    } 
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                    }
                }
               
                Console.Read();
            }
        }
    }
}
