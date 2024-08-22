using SubscriptionManagement.Business.Enums;
using SubscriptionManagement.Business.Models;

namespace SubscriptionManagement.Business.Dtos
{
    public record SubscriptionDto(string Id, string ProductId, string ProductName, string Status, string PurchaseUnit, int Quantity, string ValidTo);
}
