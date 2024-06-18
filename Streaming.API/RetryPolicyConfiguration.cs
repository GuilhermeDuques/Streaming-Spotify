using Polly; 
using Polly.Extensions.Http; 
using System.ComponentModel.DataAnnotations; 
using System.Net; 

namespace Streaming.API
{
    public class RetryPolicyConfiguration
    {
        private const int MAX_RETRY = 5; // Número máximo de tentativas de retry.

        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() // Método para configurar e retornar uma política de retry para solicitações HTTP.
        {
            return HttpPolicyExtensions
                    .HandleTransientHttpError() // Lida com erros HTTP transitórios.
                    .OrResult(m => m.StatusCode == HttpStatusCode.NotFound) // Lida com o status HTTP NotFound.
                    .WaitAndRetryAsync(MAX_RETRY, retries => TimeSpan.FromSeconds(
                        Math.Pow(2, retries) // Calcula o intervalo de retry exponencialmente crescente.
                    ));
        }
    }
}
