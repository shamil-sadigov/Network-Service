using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPWebService.Options
{
    public class GeoliteOptions
    {
        public string Scheme { get; set; }
        public string Host { get; set; }
        public string Path { get; set; }
        public string EditionId { get; set; }
        public string LicenceKey { get; set; }
        public string Suffix { get; set; }

        public Uri Uri
        {
            get 
            {
                var builder = new UriBuilder();
                builder.Scheme = Scheme;
                builder.Host = Host;
                builder.Path = Path;
                builder.Query = $"edition_id={EditionId}&license_key={LicenceKey}&suffix={Suffix}";
                return builder.Uri;
            }
        }
        
    }
}
