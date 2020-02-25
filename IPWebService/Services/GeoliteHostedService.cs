using IPWebService.Persistence;
using IPWebService.Services.Geolite;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IPWebService.Services
{
    // Service responsible for:
    // - migration MaxMind database to PostgreSQL
    // - update PostgreSQL database on schedule
    public class GeoliteHostedService : BackgroundService
    {
        private readonly IServiceProvider provider;
        private Timer timer;
        private ApplicationContext dbContext;
        private IGeoliteManager geoliteManager;
        private IWebHostEnvironment hostEnvironment;
        private IConnectionStringManager connectionManager;

        public GeoliteHostedService(IServiceProvider provider)
        {
            this.provider = provider;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = provider.CreateScope())  
            {
                dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                geoliteManager = scope.ServiceProvider.GetRequiredService<IGeoliteManager>();
                hostEnvironment = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
                connectionManager = scope.ServiceProvider.GetRequiredService<IConnectionStringManager>();

                


                // update every 7 days
                int updateIntervalInMilliseconds = TimeSpan.FromDays(7).Milliseconds; 
                dbContext.Database.EnsureCreated();

                if (!dbContext.GeoObjects.Any())
                {
                    FillDatabase();

                    // install timer to UpdateDatabase every 7 days
                    timer = new Timer(async (obj) => 
                                       await UpdateDatabaseAsync(), null, updateIntervalInMilliseconds, updateIntervalInMilliseconds);
                }
                else
                {
                    GeoObject geoObject = await dbContext.GeoObjects.FirstOrDefaultAsync();

                    DateTime lastUpdateTime = geoObject.UpdateTime;
                    DateTime currentTime = DateTime.UtcNow;
                    DateTime nextUpdateTime = CalculateNextUpdateTime(currentTime);
                    TimeSpan dayPassed = currentTime - lastUpdateTime;
                    DateTime dateToUpdate = nextUpdateTime - dayPassed;
                    TimeSpan daysLeft = dateToUpdate - currentTime;


                    if (daysLeft.Days > 0)
                    {
                        // install timer to UpdateDatabase when left days will pass and set interval to repeat this action every 7 days
                        // so we update our DB every week (Wednesday)
                        timer = new Timer(async (obj) =>                  
                                        await UpdateDatabaseAsync(), null, daysLeft.Milliseconds, updateIntervalInMilliseconds);

                    }
                    else
                    {   // install timer to UpdateDatabase now and set interval to repeat this action every 7 days
                        // so we update our DB every week (Wednesday)
                        timer = new Timer(async (obj) =>
                                       await UpdateDatabaseAsync(), null, 0, updateIntervalInMilliseconds);
                    }
                }
            }


            DateTime CalculateNextUpdateTime(DateTime currentTime)
                         => currentTime.DayOfWeek switch
                         {
                             DayOfWeek.Sunday => currentTime + TimeSpan.FromDays(3),
                             DayOfWeek.Monday => currentTime + TimeSpan.FromDays(2),
                             DayOfWeek.Tuesday => currentTime + TimeSpan.FromDays(1),
                             DayOfWeek.Wednesday => currentTime,
                             DayOfWeek.Thursday => currentTime + TimeSpan.FromDays(6),
                             DayOfWeek.Friday => currentTime + TimeSpan.FromDays(5),
                             DayOfWeek.Saturday => currentTime + TimeSpan.FromDays(4),
                             _ => currentTime
                         };

            // since we have no data in DB we should not call this method in async mode
            // because we first should migrate data to our DB and don't let any controller to be able to access DB before we 
            // so this is why it's better to do it in sync
            // because we don't want our API work when DB is empty
            void FillDatabase()
            {
                var root = hostEnvironment.ContentRootPath;
                var directory = Path.Combine(root, "Geolite");
                Directory.CreateDirectory(directory);

                string geoliteDb = geoliteManager.DownloadDbFileAsync(directory).Result;
                geoliteManager.MigrateToDbContext(geoliteDb, dbContext).Wait();
            }


            async Task UpdateDatabaseAsync()
            {
                var root = hostEnvironment.ContentRootPath;
                var directory = Path.Combine(root, "Geolite");
                Directory.CreateDirectory(directory);

                string geoliteDb = await geoliteManager.DownloadDbFileAsync(directory);

                #region Migrate geoliteDb to new Database

                string newDatabaseConnection = connectionManager.GenerateNewConnectionString();

                var newDbConnectionBuilder = new DbContextOptionsBuilder<ApplicationContext>();
                newDbConnectionBuilder.UseNpgsql(newDatabaseConnection);

                using (var context = new ApplicationContext(newDbConnectionBuilder.Options))
                {
                    await context.Database.EnsureCreatedAsync();
                    await geoliteManager.MigrateToDbContext(geoliteDb, context);
                }

                #endregion


                // get currentDbConnection so that to delete
                // because we already created new DB
                string outdatedConnectionString = connectionManager.ConnectionString;

                // set CurrentConnectionString to our new DB
                connectionManager.ConnectionString = newDatabaseConnection;


                #region Delete outdated Database

                var outdatedDbBuilder = new DbContextOptionsBuilder<ApplicationContext>();
                outdatedDbBuilder.UseNpgsql(outdatedConnectionString);

                using (var outdatedContext = new ApplicationContext(outdatedDbBuilder.Options))
                {
                    await outdatedContext.Database.EnsureDeletedAsync();
                }

                #endregion
            }

        }
    }
}
