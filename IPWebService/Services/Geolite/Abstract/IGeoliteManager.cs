using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace IPWebService.Services.Geolite
{
    public interface IGeoliteManager
    {
        /// <summary>
        /// Downloads db file from Geolite web service in specified directory and return path to downloaded database file
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        Task<string> DownloadDbFileAsync(string directoryPath);

        /// <summary>
        /// Migrates geolite database to specified DbContext
        /// </summary>
        /// <param name="geoliteDbFile"></param>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        Task MigrateToDbContext(string geoliteDbFile, DbContext dbContext);
    }
}
