using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SubscriptionManagement.Business.Models;
using SubscriptionManagement.Business.Repositories;
using SubscriptionManagement.Infrastructure.Data;
using System.ComponentModel.DataAnnotations;

namespace SubscriptionManagement.Infrastructure.Repositories
{
    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(AppDbContext context, ILogger<SubscriptionRepository> logger) : base(context, logger)
        {

        }

        public async Task<IEnumerable<Subscription>> GetAllForAccountAsync(Guid accountId)
        {
            return await _context.Subscriptions
                    .AsNoTracking()
                    .Where(s => s.AccountId == accountId && s.Status == Business.Enums.SubscriptionState.Active)
                    .ToListAsync();
        }
    }
}
