﻿using Streaming.Domain.Streaming;
using Streaming.Domain.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Streaming.Domain.Account
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public List<Cartao> Cartoes { get; set; } = new List<Cartao>();
        public List<Playlist> Playlists { get; set; } = new List<Playlist>();
        public List<Assinatura> Assinaturas { get; set; } = new List<Assinatura>();

        public void Criar(string nome, Plano plano, Cartao cartao)
        {
            this.Nome = nome;
            this.AssinarPlano(plano, cartao);
            this.AdicionarCartao(cartao);
            this.CriarPlaylist();
        }

        public void CriarPlaylist(string nome = "Favoritas", bool publica = false)
        {
            this.Playlists.Add(new Playlist()
            {
                Id = Guid.NewGuid(),
                Nome = nome,
                Publica = publica,
                Usuario = this,
            });
        }

        private void AdicionarCartao(Cartao cartao)
        {
            this.Cartoes.Add(cartao);
        }

        // Altere o nível de acesso de private para internal
        public void AssinarPlano(Plano plano, Cartao cartao)
        {
            cartao.CriarTransacao(plano.Nome, plano.Valor, plano.Descricao);

            if (this.Assinaturas.Any(x => x.Ativo))
            {
                var planoAtivo = this.Assinaturas.FirstOrDefault(x => x.Ativo);
                if (planoAtivo != null)
                {
                    planoAtivo.Ativo = false;
                }
            }

            this.Assinaturas.Add(new Assinatura()
            {
                Ativo = true,
                DtAssinatura = DateTime.Now,
                Plano = plano,
                Id = Guid.NewGuid()
            });
        }

        public void FavoritarMusica(Musica musica, string playlistNome = "Favoritas")
        {
            var playlist = ObterPlaylistPorNome(playlistNome);
            playlist.Musicas.Add(musica);
        }

        public void DesfavoritarMusica(Musica musica, string playlistNome = "Favoritas")
        {
            var playlist = ObterPlaylistPorNome(playlistNome);
            var musicaFav = playlist.Musicas.FirstOrDefault(x => x.Id == musica.Id);
            if (musicaFav == null)
            {
                throw new Exception("Não encontrei a musica");
            }
            playlist.Musicas.Remove(musicaFav);
        }

        private Playlist ObterPlaylistPorNome(string playlistNome)
        {
            var playlist = this.Playlists.FirstOrDefault(x => x.Nome == playlistNome);
            if (playlist == null)
            {
                throw new Exception("Não encontrei a playlist");
            }
            return playlist;
        }
    }
}
