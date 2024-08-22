using SubscriptionManagement.Business.Common;
using SubscriptionManagement.Infrastructure.Repositories;

namespace SubscriptionManagement.Business.Services
{
    public interface IOrderService
    {
        Task<Result<bool, Error>> OrderNew(Guid accountId, string email, Guid productSKU, int amount, DateTime validTo);
        Task<Result<bool, Error>> UpdateQuatity(Guid subscriptionId, string email, Guid productSKU, int newAmount);
        Task<Result<bool, Error>> ExtendSubscription(Guid subscriptionId, string email, Guid productSKU, DateTime newDate);
        Task<Result<bool, Error>> CancelSubscription(Guid subscriptionId, string email, Guid productSKU);
    }
}
