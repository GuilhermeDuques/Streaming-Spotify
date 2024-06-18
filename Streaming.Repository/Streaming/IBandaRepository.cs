using Streaming.Domain.Streaming;

namespace Streaming.Repository.Streaming
{
    public interface IBandaRepository
    {
        Musica GetMusica(Guid idMusica);
    }
}