using IPWebService.Options;
using IPWebService.Services.Geolite;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using static IPWebService.Helpers.Guard;

namespace IPWebService.Services
{
    public class GeoliteHttpClient : IGeoliteHttpClient
    {
        private readonly HttpClient httpClient;
        private readonly GeoliteUrlOptions geoliteUrl;
        private const string geoliteDbFileName = "geolite.tar.gz";


        public GeoliteHttpClient(HttpClient httpClient, 
                             IOptionsSnapshot<GeoliteUrlOptions> optionsSnapshot)
        {
            if (optionsSnapshot.Value is null)
                NullArgument.Throw(nameof(GeoliteUrlOptions));

            this.httpClient = httpClient;
            geoliteUrl = optionsSnapshot.Value;


        }

        /// <summary>
        /// Comments reagarding this method is specified in interface
        /// </summary>
        /// <param name="destinationPath"></param>
        /// <returns></returns>
        public async Task<string> PullGeoliteDataBaseAsync(string destinationPath)
        {
            if (string.IsNullOrEmpty(destinationPath))
                NullArgument.Throw(nameof(destinationPath));

            if (!Directory.Exists(destinationPath))
                throw new DirectoryNotFoundException($"Following directory was not found {destinationPath}");

            var httpResponse = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, geoliteUrl.Uri));
            httpResponse.EnsureSuccessStatusCode();

            string geoliteDbPath = Path.Combine(destinationPath, geoliteDbFileName);

            using (Stream stream = await httpResponse.Content.ReadAsStreamAsync())
            using (var fileStream = File.Create(geoliteDbPath))
            {
                if (stream.CanRead)
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    fileStream.Write(buffer, 0, buffer.Length);
                }
                else
                    throw new HttpRequestException("HttpClient returned invalid content. Gzip expected");
            }

            return geoliteDbPath;
        }
    }
}