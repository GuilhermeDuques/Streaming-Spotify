using Streaming.Domain.Account;

namespace Streaming.Repository.Account
{
    public interface IUsuarioRepository
    {
        Usuario GetUsuario(Guid id);
        void Remove(Usuario usuario);
        void Save(Usuario usuario);
        void Update(Usuario usuario);
    }
}