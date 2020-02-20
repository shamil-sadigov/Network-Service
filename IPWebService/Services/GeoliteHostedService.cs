using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IPWebService.Services
{
    public class GeoliteHostedService : IHostedService
    {
        public GeoliteHostedService(IServiceProvider serviceProvider)
        {
            Console.WriteLine("Hosted service "+ Thread.CurrentThread.ManagedThreadId);
        }



        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
