using Microsoft.AspNetCore.Mvc;
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
            var response = await _client.GetAsync("https://localhost:5003/summary");
            var r = response.EnsureSuccessStatusCode();
            return await r.Content.ReadAsStringAsync();
        }
    }
}
