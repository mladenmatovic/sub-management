using SubscriptionManagement.Business.Models;

namespace SubscriptionManagement.Business.Repositories
{    
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<IEnumerable<Account>> GetAllForCustomerAsync(Guid customerId);
    }
}
