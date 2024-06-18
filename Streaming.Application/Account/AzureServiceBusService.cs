using Azure.Messaging.ServiceBus; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Text.Json; 
using System.Threading.Tasks; 

namespace Streaming.Application.Account
{
    public class AzureServiceBusService : IAzureServiceBusService
    {
        private string ConnectionString = "Endpoint=sb://streaming-spotify.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=V3PuECVrHQpNwKQKkjw2lvqTBATXdWwRu+ASbEYTEms="; // String de conexão com o Azure Service Bus.

        public AzureServiceBusService() { } // Construtor padrão.

        public async Task SendMessage(Notificacao notificacao) // Método para enviar uma mensagem para o Azure Service Bus.
        {
            ServiceBusClient client; // Cliente para interagir com o serviço Azure Service Bus.
            ServiceBusSender sender; // Objeto para enviar mensagens para uma fila ou tópico.

            client = new ServiceBusClient(ConnectionString); // Inicializa o cliente com a string de conexão.

            sender = client.CreateSender("notificacao"); // Cria um sender para a fila "notificacao".

            var body = JsonSerializer.Serialize(notificacao); // Serializa a notificação em formato JSON.

            var message = new ServiceBusMessage(body); // Cria uma mensagem do Azure Service Bus com o corpo serializado.

            await sender.SendMessageAsync(message); // Envia a mensagem para o Azure Service Bus.
        }
    }
}
