using Confluent.Kafka;
using Core.Contract;
using Newtonsoft.Json;

namespace Core.Kafka
{
    public static class EventExtensions
    {
        public static object ToEvent(this ConsumeResult<string, string> consumeResult)
        {
            var eventType = ContractExtensions.GetContractType(consumeResult.Message.Key);
            ArgumentNullException.ThrowIfNull(eventType);
            return JsonConvert.DeserializeObject(consumeResult.Message.Value, eventType);
        }
    }
}
