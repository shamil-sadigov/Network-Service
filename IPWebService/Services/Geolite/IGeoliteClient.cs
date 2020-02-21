using System.Threading.Tasks;

namespace IPWebService.Services
{
    public interface IGeoliteClient
    {
        /// <summary>
        /// Pulls geolite db from Maxmind service, saves in specified destination and returns DB file name
        /// If DB file already exist in destination, then it will be replaced by new DB file
        /// </summary>
        /// <param name="destinationPath">Directory where DB file is goind to be saved in</param>
        /// <returns>DB file path</returns>
        Task<string> PullGeoliteDataBase(string destinationPath);
    }
}