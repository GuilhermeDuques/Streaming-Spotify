
namespace Streaming.Application.Exceptions
{
    public class PlanoNaoEncontradoException : Exception
    {
        public PlanoNaoEncontradoException(string message) : base(message) { }
    }

    public class UsuarioNaoEncontradoException : Exception
    {
        public UsuarioNaoEncontradoException(string message) : base(message) { }
    }

    public class MusicaNaoEncontradaException : Exception
    {
        public MusicaNaoEncontradaException(string message) : base(message) { }
    }
}
