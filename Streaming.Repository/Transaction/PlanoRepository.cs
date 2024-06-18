using Streaming.Domain.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streaming.Repository.Transaction
{
    public class PlanoRepository : IPlanoRepository
    {
        private StreamingContext streamingContext;

        public PlanoRepository(StreamingContext streamingContext)
        {
            this.streamingContext = streamingContext;
        }

        public Plano GetPlanoById(Guid planoId)
        {
            return new Plano()
            {
                Id = new Guid("6a324c2b-1ba9-4d84-a0e7-8d6d0cc2c6e7"),
                Nome = "Plano Basico",
                Descricao = "Plano basico spotify com anuncios",
                Valor = 29.99M
            };
        }
    }
}
