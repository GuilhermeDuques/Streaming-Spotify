using Streaming.Domain.Streaming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Streaming.Repository.Streaming
{
    public class BandaRepository
    {
        private readonly IHttpClientFactory _httpClientFactory = null;
        private int retries = 1;

        public BandaRepository(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }

        public Musica GetMusica(Guid idMusica)
        {

            string url = $"/musica/{idMusica}";

            HttpClient client = this._httpClientFactory.CreateClient("musicaApiServer");

            var response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode == false)
                throw new Exception("Não consegui pesquisar a musica");

            var content = response.Content.ReadAsStringAsync().Result;

            return JsonSerializer.Deserialize<Musica>(content);



        }
    }
}
