using Dafda.Producing;
using System.Threading.Tasks;

namespace SomeOtherApi
{
    public class MessageProducer
    {
        private readonly Producer producer;

        public MessageProducer(Producer producer)
        {
            this.producer = producer;
        }

        public async Task Produce(SomeMessage message)
        {
            await producer.Produce(message);
        }
    }
}