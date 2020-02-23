using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace IPWebService.Services.Geolite
{
    public interface IGeoliteManager
    {
        Task<string> InstallDbFileAsync(string directoryPath);
        Task MigrateToDbContext(string geoliteDbFile, DbContext dbContext);
    }
}
