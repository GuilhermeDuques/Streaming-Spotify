// See https://aka.ms/new-console-template for more information

using Azure.Messaging.ServiceBus;

string ConnectionString = "";

ServiceBusClient client;
ServiceBusProcessor processor;


client = new ServiceBusClient(ConnectionString);

processor = client.CreateProcessor("notificacao");

processor.ProcessMessageAsync += MessageHandler;
processor.ProcessErrorAsync += ErrorHandler;

await processor.StartProcessingAsync();

Console.WriteLine("Processando mensagem. Aperte qualquer tecla para sair");
Console.ReadKey();

await processor.StopProcessingAsync();


async Task MessageHandler(ProcessMessageEventArgs args)
{
    string body = args.Message.Body.ToString();
    Console.WriteLine($"Received: {body}");

    // complete the message. message is deleted from the queue. 
    await args.CompleteMessageAsync(args.Message);
}

Task ErrorHandler(ProcessErrorEventArgs args)
{
    Console.WriteLine(args.Exception.ToString());
    return Task.CompletedTask;
}

//Endpoint=sb://streaming-spotify.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=V3PuECVrHQpNwKQKkjw2lvqTBATXdWwRu+ASbEYTEms=

