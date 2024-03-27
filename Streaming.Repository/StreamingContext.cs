using Microsoft.EntityFrameworkCore;
using Streaming.Domain.Account;
using Streaming.Domain.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streaming.Repository
{
    public class StreamingContext : DbContext
    {

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Plano> Planos { get; set; }

        public StreamingContext(DbContextOptions<StreamingContext> options) : base(options)
        {

        }
    }
}
