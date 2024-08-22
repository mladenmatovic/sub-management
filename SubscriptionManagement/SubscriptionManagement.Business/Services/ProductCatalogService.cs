using Microsoft.Extensions.Logging;
using SubscriptionManagement.Business.Models.CCP;
using SubscriptionManagement.Business.Repositories;

namespace SubscriptionManagement.Business.Services
{
    internal class ProductCatalogService : IProductCatalogService
    {
        private readonly ICloudPartnerRepository _cloudPartnerRepository;
        private readonly ILogger<ProductCatalogService> _logger;

        public ProductCatalogService(
            ICloudPartnerRepository cloudPartnerRepository,
            ILogger<ProductCatalogService> logger)
        {
            _cloudPartnerRepository = cloudPartnerRepository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<CCPProduct>> GetAllForAccount(Guid accountId)
        {
            try
            {
                var catalog = await _cloudPartnerRepository.GetProductCatalogAsync();
                _logger.LogInformation($"Getting product catalog for account: {accountId}");
                return catalog.Select(a=> a.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error Getting product catalog for account: {accountId}");
                throw;
            }
        }
    }
}
