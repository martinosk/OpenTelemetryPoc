using Microsoft.AspNetCore.Mvc;
using SomeMessages;
using System;
using System.Threading.Tasks;

namespace SomeOtherApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SummaryController : ControllerBase
    {
        private readonly MessageProducer producer;

        private static readonly string[] Summaries = new[]
          {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public SummaryController(MessageProducer producer)
        {
            this.producer = producer;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            var rng = new Random();
            await producer.Produce(new SomeMessage());
            return Summaries[rng.Next(Summaries.Length)];
        }
    }
}
