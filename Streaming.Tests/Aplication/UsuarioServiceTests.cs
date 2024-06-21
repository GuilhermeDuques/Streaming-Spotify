﻿using Moq;
using Streaming.Application.Account;
using Streaming.Application.Exceptions;
using Streaming.Domain.Account;
using Streaming.Domain.Streaming;
using Streaming.Domain.Transaction;
using Streaming.Repository.Account;
using Streaming.Repository.Streaming;
using Streaming.Repository.Transaction;
using System;
using Xunit;

namespace Streaming.Tests.Application
{
    public class UsuarioServiceTests
    {
        private readonly Mock<IUsuarioRepository> usuarioRepositoryMock;
        private readonly Mock<IPlanoRepository> planoRepositoryMock;
        private readonly Mock<IBandaRepository> bandaRepositoryMock;
        private readonly Mock<IAzureServiceBusService> azureServiceBusServiceMock;
        private readonly UsuarioService usuarioService;

        public UsuarioServiceTests()
        {
            usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            planoRepositoryMock = new Mock<IPlanoRepository>();
            bandaRepositoryMock = new Mock<IBandaRepository>();
            azureServiceBusServiceMock = new Mock<IAzureServiceBusService>();
            usuarioService = new UsuarioService(usuarioRepositoryMock.Object, planoRepositoryMock.Object, bandaRepositoryMock.Object, azureServiceBusServiceMock.Object);
        }

        [Fact]
        public void CriarConta_DeveCriarUsuarioComSucesso_SePlanoExistir()
        {
            // Arrange
            var planoId = Guid.NewGuid();
            var plano = new Plano { Id = planoId, Valor = 50 };
            planoRepositoryMock.Setup(repo => repo.GetPlanoById(planoId)).Returns(plano);

            var nome = "Teste";
            var cartao = new Cartao();
            var usuario = new Usuario();

            // Act
            var resultado = usuarioService.CriarConta(nome, planoId, cartao);

            // Assert
            Assert.NotNull(resultado);
            usuarioRepositoryMock.Verify(repo => repo.Save(It.IsAny<Usuario>()), Times.Once);
            azureServiceBusServiceMock.Verify(service => service.SendMessage(It.IsAny<Notificacao>()), Times.Once);
        }

        [Fact]
        public void CriarConta_DeveLancarPlanoNaoEncontradoException_SePlanoNaoExistir()
        {
            // Arrange
            var planoId = Guid.NewGuid();
            planoRepositoryMock.Setup(repo => repo.GetPlanoById(planoId)).Returns((Plano)null);

            // Act & Assert
            var ex = Assert.Throws<PlanoNaoEncontradoException>(() => usuarioService.CriarConta("Nome", planoId, new Cartao()));
            Assert.Equal("Plano não encontrado", ex.Message);
        }

        [Fact]
        public void Obter_DeveRetornarUsuario_SeUsuarioExistir()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var usuario = new Usuario { Id = usuarioId };
            usuarioRepositoryMock.Setup(repo => repo.GetUsuario(usuarioId)).Returns(usuario);

            // Act
            var resultado = usuarioService.Obter(usuarioId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(usuarioId, resultado.Id);
        }

        [Fact]
        public void Obter_DeveLancarUsuarioNaoEncontradoException_SeUsuarioNaoExistir()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            usuarioRepositoryMock.Setup(repo => repo.GetUsuario(usuarioId)).Returns((Usuario)null);

            // Act & Assert
            var ex = Assert.Throws<UsuarioNaoEncontradoException>(() => usuarioService.Obter(usuarioId));
            Assert.Equal("Usuário não encontrado", ex.Message);
        }

        [Fact]
        public void FavoritarMusica_DeveAdicionarMusicaAosFavoritos_SeUsuarioEMusicaExistirem()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var musicaId = Guid.NewGuid();
            var usuario = new Usuario();
            var musica = new Musica();

            usuarioRepositoryMock.Setup(repo => repo.GetUsuario(usuarioId)).Returns(usuario);
            bandaRepositoryMock.Setup(repo => repo.GetMusica(musicaId)).Returns(musica);

            // Act
            usuarioService.FavoritarMusica(usuarioId, musicaId);

            // Assert
            usuarioRepositoryMock.Verify(repo => repo.Update(usuario), Times.Once);
        }

        [Fact]
        public void FavoritarMusica_DeveLancarUsuarioNaoEncontradoException_SeUsuarioNaoExistir()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            usuarioRepositoryMock.Setup(repo => repo.GetUsuario(usuarioId)).Returns((Usuario)null);

            // Act & Assert
            var ex = Assert.Throws<UsuarioNaoEncontradoException>(() => usuarioService.FavoritarMusica(usuarioId, Guid.NewGuid()));
            Assert.Equal("Usuário não encontrado", ex.Message);
        }

        [Fact]
        public void FavoritarMusica_DeveLancarMusicaNaoEncontradaException_SeMusicaNaoExistir()
        {
            // Arrange
            var usuario = new Usuario();
            var usuarioId = Guid.NewGuid();
            var musicaId = Guid.NewGuid();

            usuarioRepositoryMock.Setup(repo => repo.GetUsuario(usuarioId)).Returns(usuario);
            bandaRepositoryMock.Setup(repo => repo.GetMusica(musicaId)).Returns((Musica)null);

            // Act & Assert
            var ex = Assert.Throws<MusicaNaoEncontradaException>(() => usuarioService.FavoritarMusica(usuarioId, musicaId));
            Assert.Equal("Música não encontrada", ex.Message);
        }

        [Fact]
        public void DesfavoritarMusica_DeveRemoverMusicaDosFavoritos_SeUsuarioEMusicaExistirem()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var musicaId = Guid.NewGuid();
            var usuario = new Usuario();
            var musica = new Musica();

            usuarioRepositoryMock.Setup(repo => repo.GetUsuario(usuarioId)).Returns(usuario);
            bandaRepositoryMock.Setup(repo => repo.GetMusica(musicaId)).Returns(musica);

            // Act
            usuarioService.DesfavoritarMusica(usuarioId, musicaId);

            // Assert
            usuarioRepositoryMock.Verify(repo => repo.Update(usuario), Times.Once);
        }

        [Fact]
        public void DesfavoritarMusica_DeveLancarUsuarioNaoEncontradoException_SeUsuarioNaoExistir()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            usuarioRepositoryMock.Setup(repo => repo.GetUsuario(usuarioId)).Returns((Usuario)null);

            // Act & Assert
            var ex = Assert.Throws<UsuarioNaoEncontradoException>(() => usuarioService.DesfavoritarMusica(usuarioId, Guid.NewGuid()));
            Assert.Equal("Usuário não encontrado", ex.Message);
        }

        [Fact]
        public void DesfavoritarMusica_DeveLancarMusicaNaoEncontradaException_SeMusicaNaoExistir()
        {
            // Arrange
            var usuario = new Usuario();
            var usuarioId = Guid.NewGuid();
            var musicaId = Guid.NewGuid();

            usuarioRepositoryMock.Setup(repo => repo.GetUsuario(usuarioId)).Returns(usuario);
            bandaRepositoryMock.Setup(repo => repo.GetMusica(musicaId)).Returns((Musica)null);

            // Act & Assert
            var ex = Assert.Throws<MusicaNaoEncontradaException>(() => usuarioService.DesfavoritarMusica(usuarioId, musicaId));
            Assert.Equal("Música não encontrada", ex.Message);
        }
    }
}
