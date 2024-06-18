using Streaming.Domain.Transaction;

namespace Streaming.Repository.Transaction
{
    public interface IPlanoRepository
    {
        Plano GetPlanoById(Guid planoId);
    }
}