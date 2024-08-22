using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SubscriptionManagement.Business.Models;
using SubscriptionManagement.Business.Repositories;
using SubscriptionManagement.Infrastructure.Data;

namespace SubscriptionManagement.Infrastructure.Repositories
{    
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(AppDbContext context, ILogger<AccountRepository> logger) : base(context, logger)
        {

        }

        public async Task<IEnumerable<Account>> GetAllForCustomerAsync(Guid customerId)
        {
            return await _context.Accounts
                    .AsNoTracking()                                   
                    .Where(a => a.CustomerId == customerId)
                    .ToListAsync();
        }
    }
}
