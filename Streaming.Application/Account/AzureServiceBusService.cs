using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Streaming.Application.Account
{
    public class AzureServiceBusService
    {
        private string ConnectionString = "Endpoint=sb://streaming-spotify.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=V3PuECVrHQpNwKQKkjw2lvqTBATXdWwRu+ASbEYTEms=";

        public AzureServiceBusService() { }

        public async Task SendMessage(Notificacao notificacao)
        {
            ServiceBusClient client;
            ServiceBusSender sender;

            client = new ServiceBusClient(ConnectionString);

            sender = client.CreateSender("notificacao");

            var body = JsonSerializer.Serialize(notificacao);

            var message = new ServiceBusMessage(body);

            await sender.SendMessageAsync(message);

        }
    }
}
