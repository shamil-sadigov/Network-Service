using IPWebService.Persistence;
using IPWebService.Services.Geolite;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IPWebService.Services
{
    public class GeoliteHostedService : BackgroundService
    {
        private readonly IServiceProvider provider;

        public GeoliteHostedService(IServiceProvider provider)
        {
            this.provider = provider;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = provider.CreateScope())  
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                var geoliteManager = scope.ServiceProvider.GetRequiredService<IGeoliteManager>();
                var hostEnvironment = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

                return;
                // we should not use async method since if we would await
                // any request can come to our controller we could not get data from DB since 
                // we haven't craeted DB yet, so it should be sync

                dbContext.Database.EnsureCreated();

                var root = hostEnvironment.ContentRootPath;
                var directory = Path.Combine(root, "Geolite");

                
                string geoliteDb = await geoliteManager.DownloadDbFileAsync(directory);

                await geoliteManager.MigrateToDbContext(geoliteDb, dbContext);


            }





        }
    }
}
