using Moq;
using Streaming.Application.Account;
using Streaming.Domain.Account;
using Streaming.Domain.Streaming;
using Streaming.Domain.Transaction;
using Streaming.Repository.Account;
using Streaming.Repository.Streaming;
using Streaming.Repository.Transaction;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Streaming.Tests.Application
{
    public class UsuarioServiceTests
    {
        [Fact]
        public void DeveCriarUmUsuarioComSucesso()
        {
            Mock<IPlanoRepository> mockPlano = new Mock<IPlanoRepository>();
            Mock<IUsuarioRepository> mockUsuarioRepository = new Mock<IUsuarioRepository>();
            Mock<IBandaRepository> mockBandaRepository = new Mock<IBandaRepository>();
            Mock<IAzureServiceBusService> mockAzureServiceBus = new Mock<IAzureServiceBusService>();

            mockPlano.Setup(x => x.GetPlanoById(It.IsAny<Guid>())).Returns(new Plano()
            {
                Nome = "Plano Dummy",
                Valor = 29M
            });

            mockUsuarioRepository.Setup(x => x.Save(It.IsAny<Usuario>()));
            mockAzureServiceBus.Setup(x => x.SendMessage(It.IsAny<Notificacao>())).Returns(Task.CompletedTask);

            UsuarioService usuarioService = new UsuarioService(mockUsuarioRepository.Object, mockPlano.Object, mockBandaRepository.Object, mockAzureServiceBus.Object);

            Cartao cartao = new Cartao { Ativo = true, Numero = "564654654", Limite = 1000 };

            var usuario = usuarioService.CriarConta("Usuario Dummy", Guid.NewGuid(), cartao);

            Assert.NotNull(usuario);
            Assert.True(usuario.Playlists.Any());
            Assert.True(usuario.Playlists.First().Nome == "Favoritas");
            Assert.True(usuario.Cartoes.Count == 1);
            Assert.True(usuario.Assinaturas.Count == 1);
            Assert.True(usuario.Assinaturas.First().Plano.Nome == "Plano Dummy");
        }

        [Fact]
        public void CriarConta_DeveLancarExcecao_QuandoPlanoNaoEncontrado()
        {
            Mock<IPlanoRepository> mockPlano = new Mock<IPlanoRepository>();
            Mock<IUsuarioRepository> mockUsuarioRepository = new Mock<IUsuarioRepository>();
            Mock<IBandaRepository> mockBandaRepository = new Mock<IBandaRepository>();
            Mock<IAzureServiceBusService> mockAzureServiceBus = new Mock<IAzureServiceBusService>();

            mockPlano.Setup(x => x.GetPlanoById(It.IsAny<Guid>())).Returns((Plano)null);

            UsuarioService usuarioService = new UsuarioService(mockUsuarioRepository.Object, mockPlano.Object, mockBandaRepository.Object, mockAzureServiceBus.Object);

            Cartao cartao = new Cartao { Ativo = true, Numero = "564654654", Limite = 1000 };

            Assert.Throws<Exception>(() => usuarioService.CriarConta("Usuario Dummy", Guid.NewGuid(), cartao));
        }

        [Fact]
        public void FavoritarMusica_DeveLancarExcecao_QuandoUsuarioNaoEncontrado()
        {
            Mock<IUsuarioRepository> mockUsuarioRepository = new Mock<IUsuarioRepository>();
            Mock<IBandaRepository> mockBandaRepository = new Mock<IBandaRepository>();
            Mock<IPlanoRepository> mockPlano = new Mock<IPlanoRepository>();
            Mock<IAzureServiceBusService> mockAzureServiceBus = new Mock<IAzureServiceBusService>();

            mockUsuarioRepository.Setup(x => x.GetUsuario(It.IsAny<Guid>())).Returns((Usuario)null);

            UsuarioService usuarioService = new UsuarioService(mockUsuarioRepository.Object, mockPlano.Object, mockBandaRepository.Object, mockAzureServiceBus.Object);

            Assert.Throws<Exception>(() => usuarioService.FavoritarMusica(Guid.NewGuid(), Guid.NewGuid()));
        }

        [Fact]
        public void FavoritarMusica_DeveLancarExcecao_QuandoMusicaNaoEncontrada()
        {
            Mock<IUsuarioRepository> mockUsuarioRepository = new Mock<IUsuarioRepository>();
            Mock<IBandaRepository> mockBandaRepository = new Mock<IBandaRepository>();
            Mock<IPlanoRepository> mockPlano = new Mock<IPlanoRepository>();
            Mock<IAzureServiceBusService> mockAzureServiceBus = new Mock<IAzureServiceBusService>();

            var usuario = new Usuario();
            mockUsuarioRepository.Setup(x => x.GetUsuario(It.IsAny<Guid>())).Returns(usuario);
            mockBandaRepository.Setup(x => x.GetMusica(It.IsAny<Guid>())).Returns((Musica)null);

            UsuarioService usuarioService = new UsuarioService(mockUsuarioRepository.Object, mockPlano.Object, mockBandaRepository.Object, mockAzureServiceBus.Object);

            Assert.Throws<Exception>(() => usuarioService.FavoritarMusica(Guid.NewGuid(), Guid.NewGuid()));
        }

        [Fact]
        public void DesfavoritarMusica_DeveLancarExcecao_QuandoUsuarioNaoEncontrado()
        {
            Mock<IUsuarioRepository> mockUsuarioRepository = new Mock<IUsuarioRepository>();
            Mock<IBandaRepository> mockBandaRepository = new Mock<IBandaRepository>();
            Mock<IPlanoRepository> mockPlano = new Mock<IPlanoRepository>();
            Mock<IAzureServiceBusService> mockAzureServiceBus = new Mock<IAzureServiceBusService>();

            mockUsuarioRepository.Setup(x => x.GetUsuario(It.IsAny<Guid>())).Returns((Usuario)null);

            UsuarioService usuarioService = new UsuarioService(mockUsuarioRepository.Object, mockPlano.Object, mockBandaRepository.Object, mockAzureServiceBus.Object);

            Assert.Throws<Exception>(() => usuarioService.DesfavoritarMusica(Guid.NewGuid(), Guid.NewGuid()));
        }

        [Fact]
        public void DesfavoritarMusica_DeveLancarExcecao_QuandoMusicaNaoEncontrada()
        {
            Mock<IUsuarioRepository> mockUsuarioRepository = new Mock<IUsuarioRepository>();
            Mock<IBandaRepository> mockBandaRepository = new Mock<IBandaRepository>();
            Mock<IPlanoRepository> mockPlano = new Mock<IPlanoRepository>();
            Mock<IAzureServiceBusService> mockAzureServiceBus = new Mock<IAzureServiceBusService>();

            var usuario = new Usuario();
            mockUsuarioRepository.Setup(x => x.GetUsuario(It.IsAny<Guid>())).Returns(usuario);
            mockBandaRepository.Setup(x => x.GetMusica(It.IsAny<Guid>())).Returns((Musica)null);

            UsuarioService usuarioService = new UsuarioService(mockUsuarioRepository.Object, mockPlano.Object, mockBandaRepository.Object, mockAzureServiceBus.Object);

            Assert.Throws<Exception>(() => usuarioService.DesfavoritarMusica(Guid.NewGuid(), Guid.NewGuid()));
        }
    }
}
