using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace IPWebService.Test.Configurations
{
    public class WebAppTestFactory:WebApplicationFactory<Startup>
    {
        protected override IHostBuilder CreateHostBuilder()
            => Program.CreateHostBuilder(args: null);
    }
}
