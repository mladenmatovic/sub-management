using SubscriptionManagement.Business.Models;

namespace SubscriptionManagement.Business.Repositories
{    
    public interface ISubscriptionRepository : IGenericRepository<Subscription>
    {
        Task<IEnumerable<Subscription>> GetAllForAccountAsync(Guid accountId);
    }
}
