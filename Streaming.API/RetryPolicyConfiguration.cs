using Polly;
using Polly.Extensions.Http;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Streaming.API
{
    public class RetryPolicyConfiguration
    {
        private const int MAX_RETRY = 5;

        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {

            return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(m => m.StatusCode == HttpStatusCode.NotFound)
                    .WaitAndRetryAsync(MAX_RETRY, retries => TimeSpan.FromSeconds(
                        Math.Pow(2, retries)
                    ));

        }
    }
}
