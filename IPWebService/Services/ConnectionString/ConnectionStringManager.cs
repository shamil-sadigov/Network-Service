using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPWebService.Services
{
    public interface IConnectionStringManager
    {
        /// <summary>
        /// Returns current connection string from IConfiguration
        /// </summary>
        string CurrentConnectionString { get; }

        /// <summary>
        /// Generates connection string for new database
        /// </summary>
        /// <returns></returns>
        string NewConnectionString(IConnectionStringBuilder stringBuilder);

        /// <summary>
        /// Set specified connectionstring in IConfiguration
        /// </summary>
        /// <param name="connectionString"></param>
        void SetConnectionString(string connectionString);
    }
}