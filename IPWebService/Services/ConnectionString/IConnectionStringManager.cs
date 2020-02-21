using IPWebService.Persistence;
using System;
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
        string NewConnectionString(AppDbOptions options);

        /// <summary>
        /// Set specified connectionstring in IConfiguration
        /// </summary>
        /// <param name="connectionString"></param>
        void SetConnectionString(string connectionString);
    }
}