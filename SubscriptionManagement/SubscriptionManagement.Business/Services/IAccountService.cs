using SubscriptionManagement.Business.Common;
using SubscriptionManagement.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionManagement.Business.Services
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAllForCustomer(Guid customer);
        Task<Result<List<Subscription>, Error>> GetAllSubscriptions(Guid accountId);
    }
}
