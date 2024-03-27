using Streaming.Domain.Streaming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streaming.Domain.Account
{
    public class Playlist
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public bool Publica { get; set; }
        public Usuario Usuario { get; set; }
        public List<Musica> Musicas { get; set; }

        public Playlist()
        {
            Musicas = new List<Musica>();
        }

    }
}
