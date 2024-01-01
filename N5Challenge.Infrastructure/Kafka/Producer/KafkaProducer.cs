using Confluent.Kafka;

namespace N5Challenge.Infrastructure.Kafka.Producer
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly IProducer<string, string> _producer;

        public KafkaProducer(ProducerConfig config)
        {
            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task ProduceMessageAsync(string topic, Message<string, string> message)
        {
            try
            {
                var deliveryResult = await _producer.ProduceAsync(topic, message);
                Console.WriteLine($"Delivered '{deliveryResult.Value}' to '{deliveryResult.TopicPartitionOffset}'");
            }
            catch (ProduceException<string, string> e)
            {
                Console.WriteLine($"Delivery failed: {e.Error.Reason}");
            }
        }
    }
}
