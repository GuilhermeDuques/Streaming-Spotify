using Streaming.Domain.Transaction;
using Streaming.Domain.Transaction.Exceptions;
using Streaming.Domain.Core;
using System;
using Xunit;

namespace Streaming.Tests.Domain
{
    public class CartaoTests
    {
        [Fact]
        public void CriarTransacao_CartaoInativo_DeveLancarExcecao()
        {
            var cartao = new Cartao
            {
                Ativo = false,
                Limite = 1000m,
                Numero = "1234 5678 9012 3456"
            };

            var exception = Assert.Throws<CartaoException>(() => cartao.CriarTransacao("Loja 1", 100m, "Compra 1"));
            Assert.Contains(exception.Errors, e => e.ErrorDescription == "Cartão não está ativo");
        }

        [Fact]
        public void CriarTransacao_LimiteInsuficiente_DeveLancarExcecao()
        {
            var cartao = new Cartao
            {
                Ativo = true,
                Limite = 50m,
                Numero = "1234 5678 9012 3456"
            };

            var exception = Assert.Throws<CartaoException>(() => cartao.CriarTransacao("Loja 1", 100m, "Compra 1"));
            Assert.Contains(exception.Errors, e => e.ErrorDescription == "Cartão não possui limite para esta transação");
        }

        [Fact]
        public void CriarTransacao_TransacoesFrequentes_DeveLancarExcecao()
        {
            var cartao = new Cartao
            {
                Ativo = true,
                Limite = 1000m,
                Numero = "1234 5678 9012 3456"
            };

            cartao.Transacoes.Add(new Transacao { DtTransacao = DateTime.Now.AddMinutes(-1) });
            cartao.Transacoes.Add(new Transacao { DtTransacao = DateTime.Now.AddMinutes(-1) });
            cartao.Transacoes.Add(new Transacao { DtTransacao = DateTime.Now.AddMinutes(-1) });

            var exception = Assert.Throws<CartaoException>(() => cartao.CriarTransacao("Loja 1", 100m, "Compra 1"));
            Assert.Contains(exception.Errors, e => e.ErrorDescription == "Cartão utilizado muitas vezes em um período curto");
        }

        [Fact]
        public void CriarTransacao_TransacaoDuplicada_DeveLancarExcecao()
        {
            var cartao = new Cartao
            {
                Ativo = true,
                Limite = 1000m,
                Numero = "1234 5678 9012 3456"
            };

            cartao.Transacoes.Add(new Transacao { DtTransacao = DateTime.Now.AddMinutes(-1), Merchant = "Loja 1", Valor = 100m });

            var exception = Assert.Throws<CartaoException>(() => cartao.CriarTransacao("Loja 1", 100m, "Compra 1"));
            Assert.Contains(exception.Errors, e => e.ErrorDescription == "Transação duplicada para o mesmo cartão e mesmo merchant");
        }

        [Fact]
        public void CriarTransacao_TransacaoValida_DeveAdicionarTransacao()
        {
            var cartao = new Cartao
            {
                Ativo = true,
                Limite = 1000m,
                Numero = "1234 5678 9012 3456"
            };

            cartao.CriarTransacao("Loja 1", 100m, "Compra 1");

            Assert.Single(cartao.Transacoes);
            Assert.Equal(900m, cartao.Limite);
        }
    }
}
