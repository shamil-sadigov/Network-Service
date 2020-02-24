using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using IPWebService.Models;
using IPWebService.Persistence;
using IPWebService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IPWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NetworkController : Controller
    {
        private readonly GeoRepository repository;

        public NetworkController(GeoRepository repo)
        {
            repository = repo;
        }



        /// <remarks>
        /// This action take an IP address and returns its geographic coordinates 
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        public async Task<ActionResult<RequestDto>> Get(RequestDto request)
        {
            // validation is made in ApiControllerAttribute
            GeoObject geoObject = IPAddress.TryParse(request.IPAddress, out IPAddress iPAddress)
                                     ?await repository.GetByIp(iPAddress) : null;

            if (geoObject is null)
                return NotFound();

            var response = (ResponseDto)geoObject;

            return Ok(response); // explicit conversion operator
        }
    }
}
