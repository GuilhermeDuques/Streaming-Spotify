using Streaming.Domain.Account;
using System;
using System.Collections.Generic;

namespace Streaming.Domain.Streaming
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

        public void AdicionarMusica(Musica musica)
        {
            Musicas.Add(musica);
        }

        public void RemoverMusica(Musica musica)
        {
            if (!Musicas.Contains(musica))
            {
                throw new Exception("A música não está na playlist.");
            }
            Musicas.Remove(musica);
        }
    }
}
