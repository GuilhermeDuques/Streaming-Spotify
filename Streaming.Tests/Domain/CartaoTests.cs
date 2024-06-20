using Streaming.Domain.Transaction;
using Streaming.Domain.Transaction.Exceptions;
using Streaming.Domain.Core;
using System;
using Xunit;

namespace Streaming.Tests
{
    public class CartaoTests
    {
        [Fact]
        public void CriarTransacao_CartaoInativo_DeveLancarExcecao()
        {
            // Arrange
            var cartao = new Cartao
            {
                Ativo = false,
                Limite = 1000m,
                Numero = "1234 5678 9012 3456"
            };

            // Act & Assert
            var exception = Assert.Throws<CartaoException>(() => cartao.CriarTransacao("Loja 1", 100m, "Compra 1"));
            Assert.Contains(exception.Errors, e => e.ErrorDescription == "Cartão não está ativo");
        }

        [Fact]
        public void CriarTransacao_LimiteInsuficiente_DeveLancarExcecao()
        {
            // Arrange
            var cartao = new Cartao
            {
                Ativo = true,
                Limite = 50m,
                Numero = "1234 5678 9012 3456"
            };

            // Act & Assert
            var exception = Assert.Throws<CartaoException>(() => cartao.CriarTransacao("Loja 1", 100m, "Compra 1"));
            Assert.Contains(exception.Errors, e => e.ErrorDescription == "Cartão não possui limite para esta transação");
        }

        [Fact]
        public void CriarTransacao_TransacoesFrequentes_DeveLancarExcecao()
        {
            // Arrange
            var cartao = new Cartao
            {
                Ativo = true,
                Limite = 1000m,
                Numero = "1234 5678 9012 3456"
            };

            cartao.Transacoes.Add(new Transacao { DtTransacao = DateTime.Now.AddMinutes(-1) });
            cartao.Transacoes.Add(new Transacao { DtTransacao = DateTime.Now.AddMinutes(-1) });
            cartao.Transacoes.Add(new Transacao { DtTransacao = DateTime.Now.AddMinutes(-1) });

            // Act & Assert
            var exception = Assert.Throws<CartaoException>(() => cartao.CriarTransacao("Loja 1", 100m, "Compra 1"));
            Assert.Contains(exception.Errors, e => e.ErrorDescription == "Cartão utilizado muitas vezes em um período curto");
        }

        [Fact]
        public void CriarTransacao_TransacaoDuplicada_DeveLancarExcecao()
        {
            // Arrange
            var cartao = new Cartao
            {
                Ativo = true,
                Limite = 1000m,
                Numero = "1234 5678 9012 3456"
            };

            cartao.Transacoes.Add(new Transacao { DtTransacao = DateTime.Now.AddMinutes(-1), Merchant = "Loja 1", Valor = 100m });

            // Act & Assert
            var exception = Assert.Throws<CartaoException>(() => cartao.CriarTransacao("Loja 1", 100m, "Compra 1"));
            Assert.Contains(exception.Errors, e => e.ErrorDescription == "Transação duplicada para o mesmo cartão e mesmo merchant");
        }

        [Fact]
        public void CriarTransacao_TransacaoValida_DeveAdicionarTransacao()
        {
            // Arrange
            var cartao = new Cartao
            {
                Ativo = true,
                Limite = 1000m,
                Numero = "1234 5678 9012 3456"
            };

            // Act
            cartao.CriarTransacao("Loja 1", 100m, "Compra 1");

            // Assert
            Assert.Single(cartao.Transacoes);
            Assert.Equal(900m, cartao.Limite);
        }
    }
}
