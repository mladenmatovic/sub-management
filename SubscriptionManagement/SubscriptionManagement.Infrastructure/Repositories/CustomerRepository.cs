using Microsoft.Extensions.Logging;
using SubscriptionManagement.Business.Models;
using SubscriptionManagement.Business.Repositories;
using SubscriptionManagement.Infrastructure.Data;

namespace SubscriptionManagement.Infrastructure.Repositories
{    
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(AppDbContext context, ILogger<CustomerRepository> logger) : base(context, logger)
        {

        }
    }
}
