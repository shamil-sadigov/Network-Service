using IPWebService.Persistence;

namespace IPWebService.Services
{
    public interface IConnectionStringBuilder
    {
        string Build(AppDbOptions ops, string dbName);
    }
}