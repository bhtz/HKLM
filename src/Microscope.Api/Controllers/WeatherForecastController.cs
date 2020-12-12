using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microscope.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Minio.DataModel;

namespace Microscope.Api.Controllers
{
    //[Authorize(Roles="mcsp_admin")]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IStorageService _storageServicee;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IStorageService storageService)
        {
            _logger = logger;
            _storageServicee = storageService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("/storage")]
        public IEnumerable<string> GetStorage()
        {
            this._storageServicee.GetObjectAsync("test", "MicrosoftTeams-image.png");
            return new []{"test"};
        }
    }
}
