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
            #region Configure GeoliteClient

         
            services.AddHttpClient<IGeoliteHttpClient, GeoliteHttpClient>();

            services.Configure<GeoliteUrlOptions>(dbOps =>
                Configuration.GetSection("GeoliteUrlOptions").Bind(dbOps));

            #endregion

            //services.AddDbContext<ApplicationContext>(ops => ops = null);


            services.AddTransient<IConnectionStringBuilder, PostgreConnStringBuilder>();
            services.AddTransient<IConnectionStringManager, PostgreConnStringManager>();

            // you may initialize your serlf injecting necessary dependencies later
            services.AddHostedService<GeoliteHostedService>();

            services.Configure<AppDbOptions>(dbOps =>
                Configuration.GetSection("DbOptions:Postgre").Bind(dbOps));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
