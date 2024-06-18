
namespace Streaming.Application.Account
{
    public interface IAzureServiceBusService
    {
        Task SendMessage(Notificacao notificacao);
    }
}