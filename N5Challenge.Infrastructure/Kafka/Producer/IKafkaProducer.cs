using Confluent.Kafka;

namespace N5Challenge.Infrastructure.Kafka.Producer
{
    public interface IKafkaProducer
    {
        public Task ProduceMessageAsync(string topic, Message<string, string> message);
    }

}
