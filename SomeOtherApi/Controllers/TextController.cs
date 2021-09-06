using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SomeMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SomeOtherApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TextController : ControllerBase
    {
        private readonly ILogger<TextController> _logger;
        private readonly MessageProducer producer;


        public TextController(ILogger<TextController> logger, MessageProducer producer)
        {
            _logger = logger;
            this.producer = producer;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            await producer.Produce(new SomeMessage());
            return "Hello";
        }
    }
}
