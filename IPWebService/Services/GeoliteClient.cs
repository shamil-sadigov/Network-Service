using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace IPWebService.Services
{
    public class GeoliteClient : IGeoliteClient
    {
        private readonly HttpClient httpClient;

        public GeoliteClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;


            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
        }

        public void PullGeoliteDB()
        {

        }
    }
}
