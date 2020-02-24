using IPWebService.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IPWebService.Services
{
    public class GeoRepository
    {
        private readonly ApplicationContext context;

        public GeoRepository(ApplicationContext applicationContext)
        {
            context = applicationContext;
        }


        public async Task<GeoObject> GetByIp(IPAddress address)
                => await context.GeoObjects.AsNoTracking()
                                           .FirstOrDefaultAsync(x => x.IPAddress == address);
        
    }
}
