using Streaming.Domain.Account;
using Streaming.Domain.Transaction.Exceptions;
using Streaming.Domain.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
