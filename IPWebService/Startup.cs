using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IPWebService.Options;
using IPWebService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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


            GeoliteOptions options = Configuration.GetSection(nameof(GeoliteOptions))
                                         .Get<GeoliteOptions>();

            services.AddHttpClient<IGeoliteClient, GeoliteClient>
                (ops => ops.BaseAddress = options.Uri);




            #endregion


            // you may initialize your serlf injecting necessary dependencies later
            services.AddHostedService<GeoliteHostedService>();



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
