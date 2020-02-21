using IPWebService.Persistence;
using System;
using static IPWebService.Helpers.Guard;
namespace IPWebService.Services
{
    public class PostgreConnStringBuilder : IConnectionStringBuilder
    {
        public string Build(AppDbOptions ops, string dbName)
        {
            if(ops == null || string.IsNullOrEmpty(dbName))
               NullArgument.Throw(argument: nameof(AppDbOptions));

            return $"Username={ops.UserName};" +
                   $"Password={ops.Password};" +
                   $"Host={ops.Host};" +
                   $"Port={ops.Port};" +
                   $"Database={dbName}";
        }       
    }
}
