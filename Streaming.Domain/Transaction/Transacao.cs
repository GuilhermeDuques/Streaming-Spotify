using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streaming.Domain.Transaction
{
    public class Transacao
    {
        public Guid Id { get; set; }
        public DateTime DtTransacao { get; set; }
        public Decimal Valor { get; set; }

        public String Merchant { get; set; }
        public String Descricao { get; set; }
    }
}
