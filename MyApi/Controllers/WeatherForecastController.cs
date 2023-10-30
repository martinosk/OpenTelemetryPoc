using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly HttpClient _client;

        public WeatherForecastController()
        {
            _client = new HttpClient();
        }

        [HttpGet(nameof(Get))]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var result = new List<WeatherForecast>();
            for(var i = 1; i<=3; i++)
            {
                result.Add(await GetWeatherForecast(i));
            }
            return result;
        }

        [HttpGet(nameof(GetFast))]
        public async Task<IEnumerable<WeatherForecast>> GetFast()
        {
            return await Task.WhenAll(Enumerable.Range(1, 3).Select(GetWeatherForecast));
        }

        private async Task<WeatherForecast> GetWeatherForecast(int index)
        {
            var rng = new Random();
            var response = await _client.GetAsync("https://localhost:5001/api/values");
            var result = response.EnsureSuccessStatusCode();
            var body = await result.Content.ReadAsStringAsync();
            return new WeatherForecast()
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = body
            };
        }
    }
}
