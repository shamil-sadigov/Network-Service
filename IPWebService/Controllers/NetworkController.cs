using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using IPWebService.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IPWebService.Controllers
{
    [Route("api/[controller]")]
    public class NetworkController : Controller
    {
        public NetworkController(IGeoliteClient geoliteClient)
        {
            Console.WriteLine("Network Controller" + Thread.CurrentThread.ManagedThreadId);
        }


        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

    }
}
