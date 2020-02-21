using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPWebService.Services.Geolite
{
    public interface IGeoliteManager
    {
        IGeoliteHttpClient HttpClient { get; }
        IGeoliteHttpClient FileService { get; }
    }
}
