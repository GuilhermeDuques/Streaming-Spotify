using Streaming.Domain.Account;
using Streaming.Repository.Account;
using System;
using System.Linq;
using Xunit;

namespace Streaming.Tests.Repository
{
    public class UsuarioRepositoryTests
    {
        [Fact]
        public void DeveSalvarUsuarioComSucesso()
        {
            // Arrange
            var repository = new UsuarioRepository();
            var usuario = new Usuario { Nome = "Usuario Teste" };

            // Act
            repository.Save(usuario);

            // Assert
            var savedUsuario = repository.GetUsuario(usuario.Id);
            Assert.NotNull(savedUsuario);
            Assert.Equal("Usuario Teste", savedUsuario.Nome);
        }

        [Fact]
        public void DeveObterUsuarioPorId()
        {
            // Arrange
            var repository = new UsuarioRepository();
            var usuario = new Usuario { Nome = "Usuario Teste" };
            repository.Save(usuario);

            // Act
            var fetchedUsuario = repository.GetUsuario(usuario.Id);

            // Assert
            Assert.NotNull(fetchedUsuario);
            Assert.Equal(usuario.Id, fetchedUsuario.Id);
        }

        [Fact]
        public void DeveAtualizarUsuario()
        {
            // Arrange
            var repository = new UsuarioRepository();
            var usuario = new Usuario { Nome = "Usuario Teste" };
            repository.Save(usuario);

            // Act
            usuario.Nome = "Usuario Atualizado";
            repository.Update(usuario);
            var updatedUsuario = repository.GetUsuario(usuario.Id);

            // Assert
            Assert.NotNull(updatedUsuario);
            Assert.Equal("Usuario Atualizado", updatedUsuario.Nome);
        }

        [Fact]
        public void DeveRemoverUsuario()
        {
            // Arrange
            var repository = new UsuarioRepository();
            var usuario = new Usuario { Nome = "Usuario Teste" };
            repository.Save(usuario);

            // Act
            repository.Remove(usuario);
            var removedUsuario = repository.GetUsuario(usuario.Id);

            // Assert
            Assert.Null(removedUsuario);
        }

        [Fact]
        public void NaoDeveEncontrarUsuarioInexistente()
        {
            // Arrange
            var repository = new UsuarioRepository();

            // Act
            var usuario = repository.GetUsuario(Guid.NewGuid());

            // Assert
            Assert.Null(usuario);
        }
    }
}
