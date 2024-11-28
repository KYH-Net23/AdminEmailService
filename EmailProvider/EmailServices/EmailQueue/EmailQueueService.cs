using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace EmailProvider.EmailServices.EmailQueue
{
    public class EmailQueueService
    {
        private readonly ServiceBusSender _serviceBusSender;

        public EmailQueueService(ServiceBusClient client, string queueName)
        {
            _serviceBusSender = client.CreateSender(queueName);
        }

        public async Task EnQueueEmailAsync<T>(T model)
        {
            try
            {
                var messageBody = JsonConvert.SerializeObject(model);
                var message = new ServiceBusMessage(messageBody);
                await _serviceBusSender.SendMessageAsync(message);
            }
            catch
            {
                throw;
            }
        }

        public async ValueTask DisposeAsync()
        {
            await _serviceBusSender.DisposeAsync();
        }
    }
}
