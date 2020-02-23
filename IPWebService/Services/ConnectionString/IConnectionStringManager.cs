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
        string CurrentConnectionString { get; set; }

        /// <summary>
        /// Generates connection string for new database
        /// </summary>
        /// <returns></returns>
        string GenerateConnectionString(AppDbOptions options);

    }
}