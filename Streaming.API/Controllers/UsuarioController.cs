using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Streaming.API.Request;
using Streaming.API.Response;
using Streaming.Application.Account;
using Streaming.Domain.Account;
using Streaming.Domain.Transaction;

namespace Streaming.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private UsuarioService service;

        public UsuarioController(UsuarioService service)
        {
            this.service = service;
        }

        [HttpPost]
        public IActionResult Criar(CriarUsuarioRequest request)
        {
            if (ModelState.IsValid == false) return BadRequest();

            Cartao cartao = new Cartao()
            {
                Limite = request.Cartao.Limite,
                Ativo = request.Cartao.Ativo,
                Numero = request.Cartao.Numero
            };

            var usuarioCriado = this.service.CriarConta(request.Nome, request.PlanoId, cartao);
            UsuarioResponse response = UsuarioParaResponse(usuarioCriado);

            return Created($"/usuario/{response.Id}", response);

        }

        [HttpGet("{id}")]
        public IActionResult Obter(Guid id)
        {
            var usuario = this.service.Obter(id);

            if (usuario == null)
                return NotFound();

            var response = UsuarioParaResponse(usuario);

            return Ok(response);

        }

        [HttpPost("{id}/favoritar/{idMusica}")]
        public IActionResult FavoritarMusica(Guid id, Guid idMusica)
        {
            this.service.FavoritarMusica(id, idMusica);

            var usuario = this.service.Obter(id);

            var response = UsuarioParaResponse(usuario);

            return Ok(response);
        }

        [HttpPost("{id}/desfavoritar/{idMusica}")]
        public IActionResult DesfavoritarMusica(Guid id, Guid idMusica)
        {
            this.service.DesfavoritarMusica(id, idMusica);

            var usuario = this.service.Obter(id);

            var response = UsuarioParaResponse(usuario);

            return Ok(response);
        }

        private UsuarioResponse UsuarioParaResponse(Usuario usuarioCriado)
        {
            var response = new UsuarioResponse()
            {
                Id = usuarioCriado.Id,
                Nome = usuarioCriado.Nome,
                PlanoId = usuarioCriado.Assinaturas.FirstOrDefault(x => x.Ativo).Plano.Id
            };

            foreach (var item in usuarioCriado.Playlists)
            {
                var playlistResponse = new PlaylistResponse();
                playlistResponse.Id = item.Id;
                playlistResponse.Nome = item.Nome;
                response.Playlists.Add(playlistResponse);

                foreach (var musica in item.Musicas)
                {
                    playlistResponse.Musica.Add(new MusicaResponse()
                    {
                        Duracao = musica.Duracao,
                        Nome = musica.Nome,
                        Id = musica.Id
                    });
                }
            }

            return response;
        }

    }
}
