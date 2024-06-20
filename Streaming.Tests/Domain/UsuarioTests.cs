using Streaming.Domain.Account;
using Streaming.Domain.Transaction.Exceptions;
using Streaming.Domain.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Streaming.Domain.Streaming;

namespace Streaming.Tests.Domain
{
    public class UsuarioTests
    {
        [Fact]
        public void DeveCriarUmUsuarioComSucesso()
        {
            //AAA -> Arrange, Act, Assert

            Cartao cartao = new Cartao();
            cartao.Ativo = true;
            cartao.Numero = "564654654";
            cartao.Limite = 1000;

            Plano plano = new Plano();
            plano.Valor = 29.0M;
            plano.Nome = "Plano Lorem Ipsum";

            var usuario = new Usuario();
            usuario.Criar("Lorem ipsum", plano, cartao);

            Assert.NotNull(usuario);
            Assert.True(usuario.Playlists.Any());
            Assert.True(usuario.Playlists.First().Nome == "Favoritas");
            Assert.True(usuario.Cartoes.Count == 1);
            Assert.True(usuario.Assinaturas.Count == 1);
            Assert.True(usuario.Assinaturas.First().Plano.Nome == plano.Nome);
        }

        [Fact]
        public void NaoDeveCriarUsuarioCasoLimiteCartaoForMenorValorPlano()
        {
            Cartao cartao = new Cartao();
            cartao.Ativo = true;
            cartao.Numero = "564654654";
            cartao.Limite = 20;

            Plano plano = new Plano();
            plano.Valor = 29.0M;
            plano.Nome = "Plano Lorem Ipsum";

            Assert.Throws<CartaoException>(() =>
            {
                var usuario = new Usuario();
                usuario.Criar("Lorem ipsum", plano, cartao);
            });
        }

        [Fact]
        public void NaoDeveCriarUsuarioCasoCartaoEstejaInativo()
        {
            Cartao cartao = new Cartao();
            cartao.Ativo = false;
            cartao.Numero = "564654654";
            cartao.Limite = 1000;

            Plano plano = new Plano();
            plano.Valor = 29.0M;
            plano.Nome = "Plano Lorem Ipsum";

            Assert.Throws<CartaoException>(() =>
            {
                var usuario = new Usuario();
                usuario.Criar("Lorem ipsum", plano, cartao);
            });
        }

        [Fact]
        public void CriarPlaylist_DeveAdicionarNovaPlaylist()
        {
            // Arrange
            var usuario = new Usuario();

            // Act
            usuario.CriarPlaylist("Minha Playlist", true);

            // Assert
            Assert.Single(usuario.Playlists);
            var playlist = usuario.Playlists.First();
            Assert.Equal("Minha Playlist", playlist.Nome);
            Assert.True(playlist.Publica);
            Assert.Equal(usuario, playlist.Usuario);
        }

        [Fact]
        public void FavoritarMusica_DeveAdicionarMusicaNaPlaylist()
        {
            // Arrange
            var usuario = new Usuario();
            usuario.CriarPlaylist("Favoritas", false);
            var musica = new Musica { Id = Guid.NewGuid(), Nome = "Musica 1" };

            // Act
            usuario.FavoritarMusica(musica);

            // Assert
            var playlist = usuario.Playlists.First(x => x.Nome == "Favoritas");
            Assert.Single(playlist.Musicas);
            Assert.Contains(musica, playlist.Musicas);
        }

        [Fact]
        public void FavoritarMusica_DeveLancarExceptionSePlaylistNaoEncontrada()
        {
            // Arrange
            var usuario = new Usuario();
            var musica = new Musica { Id = Guid.NewGuid(), Nome = "Musica 1" };

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => usuario.FavoritarMusica(musica));
            Assert.Equal("Não encontrei a playlist", exception.Message);
        }

        [Fact]
        public void DesfavoritarMusica_DeveRemoverMusicaDaPlaylist()
        {
            // Arrange
            var usuario = new Usuario();
            usuario.CriarPlaylist("Favoritas", false);
            var musica = new Musica { Id = Guid.NewGuid(), Nome = "Musica 1" };
            usuario.FavoritarMusica(musica);

            // Act
            usuario.DesfavoritarMusica(musica);

            // Assert
            var playlist = usuario.Playlists.First(x => x.Nome == "Favoritas");
            Assert.Empty(playlist.Musicas);
        }

        [Fact]
        public void DesfavoritarMusica_DeveLancarExceptionSePlaylistNaoEncontrada()
        {
            // Arrange
            var usuario = new Usuario();
            var musica = new Musica { Id = Guid.NewGuid(), Nome = "Musica 1" };

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => usuario.DesfavoritarMusica(musica));
            Assert.Equal("Não encontrei a playlist", exception.Message);
        }

        [Fact]
        public void DesfavoritarMusica_DeveLancarExceptionSeMusicaNaoEncontrada()
        {
            // Arrange
            var usuario = new Usuario();
            usuario.CriarPlaylist("Favoritas", false);
            var musica = new Musica { Id = Guid.NewGuid(), Nome = "Musica 1" };

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => usuario.DesfavoritarMusica(musica));
            Assert.Equal("Não encontrei a musica", exception.Message);
        }
    }
}
