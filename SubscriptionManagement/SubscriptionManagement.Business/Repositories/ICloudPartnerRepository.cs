using SubscriptionManagement.Business.Models;
using SubscriptionManagement.Business.Models.CCP;
using SubscriptionManagement.Infrastructure.Repositories;

namespace SubscriptionManagement.Business.Repositories
{
    public interface ICloudPartnerRepository
    {
        Task<Dictionary<Guid, CCPProduct>> GetProductCatalogAsync();
        Task<CCPProduct?> GetProductBySKUAsync(Guid sku);
        Task<AccountResponse> CreateAccountAsync(Account account);
        Task<AccountResponse> DeleteAccountAsync(string accountEmail);
        Task<SubscriptionResponse> OrderSubscriptionAsync(string accountEmail, Guid productId, int newQuantity);
        Task<SubscriptionResponse> UpdateSubscriptionQuantityAsync(string accountEmail, Guid productId, int newQuantity);
        Task<SubscriptionResponse> CancelSubscriptionAsync(string accountEmail, string productId);
        Task<SubscriptionResponse> ExtendSubscriptionLicence(string accountEmail, string productId, DateTime newDate);

    }
}
