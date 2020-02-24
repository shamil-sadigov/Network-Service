using IPWebService.Helpers;
using IPWebService.Options;
using IPWebService.Persistence;
using IPWebService.Services;
using IPWebService.Services.Geolite;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Z.EntityFramework.Extensions;

namespace IPWebService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<GeoliteHostedService>();

            #region Configure Geolite services

            services.AddHttpClient<IGeoliteHttpClient, GeoliteHttpClient>();

            services.Configure<GeoliteUrlOptions>(dbOps =>
                Configuration.GetSection("GeoliteUrlOptions").Bind(dbOps));
            
            services.AddTransient<IConnectionStringBuilder, ConnectionStringBuilder>();
            services.AddTransient<IConnectionStringManager, ConnectionStringManager>();
            services.AddTransient<IGeoliteFileService, GeoliteFileService>();
            services.AddTransient<IGeoliteManager, GeoliteManager>();
            services.AddScoped<GeoRepository>();

            #endregion

            services.AddDbContext<ApplicationContext>((serviceProvider, dbBuilder) =>
            {
                var connStringManager = serviceProvider.GetRequiredService<IConnectionStringManager>();

                if (string.IsNullOrEmpty(connStringManager.ConnectionString))
                    connStringManager.ConnectionString = connStringManager.GenerateNewConnectionString();

                dbBuilder.UseNpgsql(connStringManager.ConnectionString);

                // this requirement is for Z.EntitFramework library since we use Bulk Insert
                // it's seems strange, but it required in documentation
                EntityFrameworkManager.ContextFactory = context => 
                        new ApplicationContext(new DbContextOptionsBuilder<ApplicationContext>()
                                                        .UseNpgsql(connStringManager.ConnectionString)
                                                        .Options);
            });

            // you may initialize your serlf injecting necessary dependencies later
            services.Configure<AppDbOptions>(dbOps =>
                Configuration.GetSection("DbOptions:Postgre").Bind(dbOps));

            services.AddSwagger();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
