using SubscriptionManagement.Business.Models.CCP;

namespace SubscriptionManagement.Business.Services
{
    public interface IProductCatalogService
    {
        Task<IEnumerable<CCPProduct>> GetAllForAccount(Guid accountId);
    }
}
