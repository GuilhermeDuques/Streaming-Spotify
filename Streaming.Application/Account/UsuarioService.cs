using Streaming.Domain.Account;
using Streaming.Domain.Streaming;
using Streaming.Domain.Transaction;
using Streaming.Repository.Account;
using Streaming.Repository.Streaming;
using Streaming.Repository.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        // Cria uma nova conta de usuário com o nome fornecido, associada ao plano especificado e cartão de crédito.
        public Usuario CriarConta(String nome, Guid planoId, Cartao cartao)
        {
            Plano plano = this.planoRepository.GetPlanoById(planoId);

            if (plano == null)
                throw new Exception("Plano não encontrado");

            Usuario usuario = new Usuario();
            usuario.Criar(nome, plano, cartao);

            this.usuarioRepository.Save(usuario);

            var notificacao = new Notificacao();
            notificacao.IdUsuario = usuario.Id;
            notificacao.Message = $"Seja bem vindo. Debitamos o valor de R$ {plano.Valor.ToString("N2")} no seu cartão";

            azureServiceBusService.SendMessage(notificacao).Wait();

            return usuario;
        }

        // Obtém um usuário pelo ID fornecido.
        public Usuario Obter(Guid id)
        {
            var usuario = this.usuarioRepository.GetUsuario(id);
            return usuario;
        }

        // Favorita uma música para um usuário específico.
        public void FavoritarMusica(Guid id, Guid idMusica)
        {
            var usuario = this.usuarioRepository.GetUsuario(id);
            if (usuario == null) throw new Exception("Não encontrei o usuario");

            var musica = VerificarMusica(idMusica);

            usuario.FavoritarMusica(musica);

            this.usuarioRepository.Update(usuario);
        }

        // Remove uma música dos favoritos de um usuário específico.
        public void DesfavoritarMusica(Guid id, Guid idMusica)
        {
            var usuario = this.usuarioRepository.GetUsuario(id);
            if (usuario == null) throw new Exception("Não encontrei o usuario");

            var musica = VerificarMusica(idMusica);

            usuario.DesfavoritarMusica(musica);

            this.usuarioRepository.Update(usuario);
        }

        // Verifica se uma música existe e a retorna pelo ID fornecido.
        private Musica VerificarMusica(Guid idMusica)
        {
            var musica = this.bandaRepository.GetMusica(idMusica);

            if (musica == null) throw new Exception("Não encontrei a musica a ser favoritada");

            return musica;
        }
    }
}

