using Streaming.Domain.Account;
using Streaming.Domain.Streaming;
using System;
using System.Linq;
using Xunit;

namespace Streaming.Tests.Domain
{
    public class PlaylistTests
    {
        // Teste para garantir que a playlist é criada corretamente
        [Fact]
        public void DeveCriarPlaylistComSucesso()
        {
            // Arrange
            var usuario = new Usuario();
            var playlist = new Playlist
            {
                Id = Guid.NewGuid(),
                Nome = "Minha Playlist",
                Publica = true,
                Usuario = usuario
            };

            // Assert
            Assert.NotNull(playlist);
            Assert.Equal("Minha Playlist", playlist.Nome);
            Assert.True(playlist.Publica);
            Assert.Equal(usuario, playlist.Usuario);
            Assert.Empty(playlist.Musicas);
        }

        // Teste para adicionar uma música à playlist
        [Fact]
        public void DeveAdicionarMusicaNaPlaylist()
        {
            // Arrange
            var playlist = new Playlist();
            var musica = new Musica { Id = Guid.NewGuid(), Nome = "Musica 1" };

            // Act
            playlist.AdicionarMusica(musica);

            // Assert
            Assert.Single(playlist.Musicas);
            Assert.Contains(musica, playlist.Musicas);
        }

        // Teste para remover uma música da playlist
        [Fact]
        public void DeveRemoverMusicaDaPlaylist()
        {
            // Arrange
            var playlist = new Playlist();
            var musica = new Musica { Id = Guid.NewGuid(), Nome = "Musica 1" };
            playlist.AdicionarMusica(musica);

            // Act
            playlist.RemoverMusica(musica);

            // Assert
            Assert.Empty(playlist.Musicas);
        }

        // Teste para garantir que uma exceção é lançada ao tentar remover uma música que não está na playlist
        [Fact]
        public void DeveLancarExceptionAoRemoverMusicaNaoEncontradaNaPlaylist()
        {
            // Arrange
            var playlist = new Playlist();
            var musica = new Musica { Id = Guid.NewGuid(), Nome = "Musica 1" };

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => playlist.RemoverMusica(musica));
            Assert.Equal("A música não está na playlist.", exception.Message);
        }

        // Teste para verificar se uma música específica está na playlist
        [Fact]
        public void DeveConterMusicaNaPlaylist()
        {
            // Arrange
            var playlist = new Playlist();
            var musica = new Musica { Id = Guid.NewGuid(), Nome = "Musica 1" };
            playlist.AdicionarMusica(musica);

            // Act
            var contemMusica = playlist.Musicas.Contains(musica);

            // Assert
            Assert.True(contemMusica);
        }

        // Teste para verificar se uma música específica não está na playlist
        [Fact]
        public void NaoDeveConterMusicaNaoAdicionadaNaPlaylist()
        {
            // Arrange
            var playlist = new Playlist();
            var musica = new Musica { Id = Guid.NewGuid(), Nome = "Musica 1" };

            // Act
            var contemMusica = playlist.Musicas.Contains(musica);

            // Assert
            Assert.False(contemMusica);
        }

        // Teste para verificar se o nome da playlist pode ser atualizado
        [Fact]
        public void DeveAtualizarNomeDaPlaylist()
        {
            // Arrange
            var playlist = new Playlist { Nome = "Nome Antigo" };

            // Act
            playlist.Nome = "Nome Novo";

            // Assert
            Assert.Equal("Nome Novo", playlist.Nome);
        }

        // Teste para verificar se a visibilidade da playlist pode ser atualizada
        [Fact]
        public void DeveAtualizarVisibilidadeDaPlaylist()
        {
            // Arrange
            var playlist = new Playlist { Publica = false };

            // Act
            playlist.Publica = true;

            // Assert
            Assert.True(playlist.Publica);
        }
    }
}
