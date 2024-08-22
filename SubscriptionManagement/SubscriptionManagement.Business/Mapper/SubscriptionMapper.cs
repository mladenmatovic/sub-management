using SubscriptionManagement.Business.Common.Helpers;
using SubscriptionManagement.Business.Dtos;
using SubscriptionManagement.Business.Models;

namespace SubscriptionManagement.Business.Mapper
{
    public static class SubscriptionMapper
    {
        public static SubscriptionDto ToDto(this Subscription subscription)
        {
            return new SubscriptionDto(
                subscription.Id.ToString(),
                subscription.ProductId,
                subscription.ProductName,
                subscription.Status.ToString(),
                PurchaseUnitMapper.ToFriendlyString(subscription.PurchaseUnit),
                subscription.Quantity,
                subscription.ValidTo.ToCustomFormattedDateString(DateFormatConstants.Short));
        }
    }
}
