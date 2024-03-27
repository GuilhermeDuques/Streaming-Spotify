using System.ComponentModel.DataAnnotations;

namespace Streaming.API.Request
{
    public class CriarUsuarioRequest
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        public Guid PlanoId { get; set; }

        [Required]
        public CartaoRequest Cartao { get; set; }

    }

    public class CartaoRequest
    {
        public Boolean Ativo { get; set; }
        public Decimal Limite { get; set; }
        public String Numero { get; set; }

    }
}
