using SubscriptionManagement.Business.Common;
using SubscriptionManagement.Business.Repositories;
using SubscriptionManagement.Business.Validation.Abstract;


namespace SubscriptionManagement.Business.Validation.Order.Handlers
{
    public class SubscriptionValidationHandler : ValidationHandler<OrderContext>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriptionValidationHandler(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public override async Task<Result<bool, Error>> Validate(OrderContext context)
        {
            var account = await _subscriptionRepository.GetByIdAsync(context.SubscriptionId);
            if (account == null)
            {
                return new Error("Subscription.Update", $"Failed to update subscription for non existent subscriptionId: {context.SubscriptionId}");
            }
            
            return _nextHandler != null ? await _nextHandler.Validate(context) : true;
        }
    }
}
