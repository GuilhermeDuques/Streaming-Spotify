using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streaming.Domain.Transaction
{
    public class Assinatura
    {
        public Guid Id { get; set; }
        public Plano Plano { get; set; }
        public Boolean Ativo { get; set; }
        public DateTime DtAssinatura { get; set; }
    }
}
