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
        string ConnectionString { get; set; }

        /// <summary>
        /// Generates connection string for new database
        /// </summary>
        /// <returns></returns>
        string GenerateNewConnectionString(AppDbOptions options);

    }
}