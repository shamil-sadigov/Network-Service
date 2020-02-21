using IPWebService.Persistence;

namespace IPWebService.Services
{
    public class PostgreConnStringBuilder : IConnectionStringBuilder
    {
        public string Build(AppDbOptions ops, string dbName)
            => $"Username={ops.UserName};" +
               $"Password={ops.Password};" +
               $"Host={ops.Host};" +
               $"Database={dbName}";
    }



}
