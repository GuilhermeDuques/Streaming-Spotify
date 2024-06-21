using Moq;
using Streaming.Domain.Transaction;
using Streaming.Repository;
using Streaming.Repository.Transaction;
using System;
using Xunit;

namespace Streaming.Tests.Repository
{
    public class PlanoRepositoryTests
    {
        [Fact]
        public void DeveObterPlanoPorId()
        {
            // Arrange
            var mockStreamingContext = new Mock<StreamingContext>();
            var repository = new PlanoRepository(mockStreamingContext.Object);
            var expectedPlanoId = new Guid("6a324c2b-1ba9-4d84-a0e7-8d6d0cc2c6e7");

            // Act
            var plano = repository.GetPlanoById(expectedPlanoId);

            // Assert
            Assert.NotNull(plano);
            Assert.Equal(expectedPlanoId, plano.Id);
            Assert.Equal("Plano Basico", plano.Nome);
            Assert.Equal("Plano basico spotify com anuncios", plano.Descricao);
            Assert.Equal(29.99M, plano.Valor);
        }

        [Fact]
        public void GetPlanoById_DeveRetornarPlanoEsperado()
        {
            // Arrange
            var mockStreamingContext = new Mock<StreamingContext>();
            var repository = new PlanoRepository(mockStreamingContext.Object);
            var expectedPlanoId = new Guid("6a324c2b-1ba9-4d84-a0e7-8d6d0cc2c6e7");

            // Act
            var plano = repository.GetPlanoById(expectedPlanoId);

            // Assert
            Assert.NotNull(plano);
            Assert.Equal(expectedPlanoId, plano.Id);
            Assert.Equal("Plano Basico", plano.Nome);
            Assert.Equal("Plano basico spotify com anuncios", plano.Descricao);
            Assert.Equal(29.99M, plano.Valor);
        }

        [Fact]
        public void GetPlanoById_DeveRetornarNullParaIdInvalido()
        {
            // Arrange
            var mockStreamingContext = new Mock<StreamingContext>();
            var repository = new PlanoRepository(mockStreamingContext.Object);
            var invalidPlanoId = new Guid("00000000-0000-0000-0000-000000000000");

            // Act
            var plano = repository.GetPlanoById(invalidPlanoId);

            // Assert
            Assert.Null(plano);
        }
    }
}
