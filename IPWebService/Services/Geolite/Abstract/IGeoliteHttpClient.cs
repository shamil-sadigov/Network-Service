using System.Threading.Tasks;

namespace IPWebService.Services
{
    public interface IGeoliteHttpClient
    {
        /// <summary>
        /// Pulls geolite db from Maxmind service, saves in specified destination and returns DB archive file name
        /// If DB file already exist in destination, then it will be replaced by new DB file
        /// </summary>
        /// <param name="destinationPath">Directory where DB file is goind to be saved in</param>
        /// <returns>DB archive file path</returns>
        Task<string> PullGeoliteDataBaseArchive(string destinationPath);
    }
}