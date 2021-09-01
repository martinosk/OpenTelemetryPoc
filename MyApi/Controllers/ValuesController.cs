using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly HttpClient _client;

        public ValuesController()
        {
            _client = new HttpClient();
        }

        [HttpGet]
        public async Task<string> Get()
        {
            var response = await _client.GetAsync("https://localhost:5003/Text");
            var r = response.EnsureSuccessStatusCode();
            return await r.Content.ReadAsStringAsync();
        }
    }
}
