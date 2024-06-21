using Streaming.Domain.Account;
using Streaming.Domain.Streaming;
using Streaming.Domain.Transaction;
using Streaming.Repository.Account;
using Streaming.Repository.Streaming;
using Streaming.Repository.Transaction;
using System;
using Streaming.Application.Exceptions;

namespace Streaming.Application.Account
{
    public class UsuarioService
    {
        private IUsuarioRepository usuarioRepository;
        private IPlanoRepository planoRepository;
        private IBandaRepository bandaRepository;
        private IAzureServiceBusService azureServiceBusService;

        public UsuarioService(IUsuarioRepository usuarioRepository, IPlanoRepository planoRepository, IBandaRepository bandaRepository, IAzureServiceBusService azureServiceBusService)
        {
            this.usuarioRepository = usuarioRepository;
            this.planoRepository = planoRepository;
            this.bandaRepository = bandaRepository;
            this.azureServiceBusService = azureServiceBusService;
        }

        public Usuario CriarConta(string nome, Guid planoId, Cartao cartao)
        {
            Plano plano = this.planoRepository.GetPlanoById(planoId);
            if (plano == null)
                throw new PlanoNaoEncontradoException("Plano não encontrado");

            Usuario usuario = new Usuario();
            usuario.Criar(nome, plano, cartao);
            this.usuarioRepository.Save(usuario);

            var notificacao = new Notificacao
            {
                IdUsuario = usuario.Id,
                Message = $"Seja bem vindo. Debitamos o valor de R$ {plano.Valor.ToString("N2")} no seu cartão"
            };

            azureServiceBusService.SendMessage(notificacao).Wait();
            return usuario;
        }

        public Usuario Obter(Guid id)
        {
            var usuario = this.usuarioRepository.GetUsuario(id);
            if (usuario == null) throw new UsuarioNaoEncontradoException("Usuário não encontrado");
            return usuario;
        }

        public void FavoritarMusica(Guid id, Guid idMusica)
        {
            var usuario = this.usuarioRepository.GetUsuario(id);
            if (usuario == null) throw new UsuarioNaoEncontradoException("Usuário não encontrado");

            var musica = VerificarMusica(idMusica);
            usuario.FavoritarMusica(musica);
            this.usuarioRepository.Update(usuario);
        }

        public void DesfavoritarMusica(Guid id, Guid idMusica)
        {
            var usuario = this.usuarioRepository.GetUsuario(id);
            if (usuario == null) throw new UsuarioNaoEncontradoException("Usuário não encontrado");

            var musica = VerificarMusica(idMusica);
            usuario.DesfavoritarMusica(musica);
            this.usuarioRepository.Update(usuario);
        }

        private Musica VerificarMusica(Guid idMusica)
        {
            var musica = this.bandaRepository.GetMusica(idMusica);
            if (musica == null) throw new MusicaNaoEncontradaException("Música não encontrada");
            return musica;
        }
    }
}
