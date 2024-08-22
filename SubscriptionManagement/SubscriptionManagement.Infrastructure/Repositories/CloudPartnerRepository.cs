using Microsoft.Extensions.Caching.Memory;
using SubscriptionManagement.Business.Models;
using SubscriptionManagement.Business.Models.CCP;
using SubscriptionManagement.Business.Repositories;

namespace SubscriptionManagement.Infrastructure.Repositories
{
    public class CloudPartnerRepository : ICloudPartnerRepository
    {
        private readonly IMemoryCache _memoryCache;
        private const string ProductCatalogCacheKey = "CCPProductCatalog";
        private const int ProductCatalogCacheDuration = 21600; // Cache duration in seconds (6 hours)
        private readonly SemaphoreSlim _productCatalogSemaphore = new SemaphoreSlim(1, 1);

        public CloudPartnerRepository(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<Dictionary<Guid, CCPProduct>> GetProductCatalogAsync()
        {            
            if (_memoryCache.TryGetValue(ProductCatalogCacheKey, out Dictionary<Guid, CCPProduct> productCatalog))
            {
                return productCatalog!;
            }
            
            await _productCatalogSemaphore.WaitAsync();
            try
            {
                // Check the cache again in case another thread already fetched the catalog
                if (_memoryCache.TryGetValue(ProductCatalogCacheKey, out productCatalog))
                {
                    return productCatalog!;
                }

                productCatalog = await GetMockedProductCatalog();
               
                _memoryCache.Set(ProductCatalogCacheKey, productCatalog, TimeSpan.FromSeconds(ProductCatalogCacheDuration));
            }
            finally
            {
                _productCatalogSemaphore.Release();
            }

            return productCatalog;
        }

        public async Task<CCPProduct?> GetProductBySKUAsync(Guid sku)
        {
            var productCatalog = await GetProductCatalogAsync();

            if (productCatalog.TryGetValue(sku, out CCPProduct? product))
            {
                return product;
            }

            return null;
        }

        private async Task<Dictionary<Guid, CCPProduct>> GetMockedProductCatalog()
        {            
            return new Dictionary<Guid, CCPProduct>
            {
                { new Guid("7a035d40-56c4-446d-bb3b-15658dd2b5ef"), new CCPProduct { SKU = new Guid("7a035d40-56c4-446d-bb3b-15658dd2b5ef"), ProductId = 1, Category = "AI", Name = "AI Builder Capacity", PurchaseUnit = "1M", Price = 10.99m } },
                { new Guid("bfce06aa-392c-4459-9a98-e9959debd262"), new CCPProduct { SKU = new Guid("bfce06aa-392c-4459-9a98-e9959debd262"), ProductId = 2, Category = "Productivity", Name = "Office 365", PurchaseUnit = "1M", Price = 12.99m } },
                { new Guid("160224cc-1361-47f8-82c4-52f679954b66"), new CCPProduct { SKU = new Guid("160224cc-1361-47f8-82c4-52f679954b66"), ProductId = 3, Category = "Productivity", Name = "Microsoft 365 E3", PurchaseUnit = "1M", Price = 32.99m } },
                { new Guid("947535f4-25d4-4339-8b8a-817587f44654"), new CCPProduct { SKU = new Guid("947535f4-25d4-4339-8b8a-817587f44654"), ProductId = 4, Category = "Analytics", Name = "Power BI Pro", PurchaseUnit = "1M", Price = 9.99m } },
                { new Guid("40bf3634-fbe4-4170-bf4d-2405b9617492"), new CCPProduct { SKU = new Guid("40bf3634-fbe4-4170-bf4d-2405b9617492"), ProductId = 5, Category = "AI", Name = "AI Builder Capacity", PurchaseUnit = "1M", Price = 10.99m } },
                { new Guid("da489328-ab71-40f3-8c12-80df2d948a48"), new CCPProduct { SKU = new Guid("da489328-ab71-40f3-8c12-80df2d948a48"), ProductId = 6, Category = "Productivity", Name = "Office 365", PurchaseUnit = "1M", Price = 12.99m } },
                { new Guid("a651cbda-c612-4e27-ac03-aea1a18f6a5f"), new CCPProduct { SKU = new Guid("a651cbda-c612-4e27-ac03-aea1a18f6a5f"), ProductId = 7, Category = "Productivity", Name = "Microsoft 365 E3", PurchaseUnit = "1M", Price = 32.99m } },
                { new Guid("1208eb39-e625-4f3d-8d8c-2b31b3074967"), new CCPProduct { SKU = new Guid("1208eb39-e625-4f3d-8d8c-2b31b3074967"), ProductId = 8, Category = "Analytics", Name = "Power BI Pro", PurchaseUnit = "1M", Price = 9.99m } },
                { new Guid("af65fe16-2710-4fda-99bf-723b3e6b1bd8"), new CCPProduct { SKU = new Guid("af65fe16-2710-4fda-99bf-723b3e6b1bd8"), ProductId = 9, Category = "AI", Name = "AI Builder Capacity", PurchaseUnit = "1M", Price = 10.99m } },
                { new Guid("e7e33391-ed09-4310-be5f-75f30acd7d0e"), new CCPProduct { SKU = new Guid("e7e33391-ed09-4310-be5f-75f30acd7d0e"), ProductId = 10, Category = "Productivity", Name = "Office 365", PurchaseUnit = "1M", Price = 12.99m } },
                { new Guid("6f0e2fae-7596-4cce-9494-1e4b581e1f55"), new CCPProduct { SKU = new Guid("6f0e2fae-7596-4cce-9494-1e4b581e1f55"), ProductId = 11, Category = "Productivity", Name = "Microsoft 365 E3", PurchaseUnit = "1M", Price = 32.99m } },
                { new Guid("42076095-57a6-46c5-a105-2bf7c71ffc62"), new CCPProduct { SKU = new Guid("42076095-57a6-46c5-a105-2bf7c71ffc62"), ProductId = 12, Category = "Analytics", Name = "Power BI Pro", PurchaseUnit = "1M", Price = 9.99m } },
                { new Guid("ad41dd18-ac48-4f2d-81fd-33f7083b3b9a"), new CCPProduct { SKU = new Guid("ad41dd18-ac48-4f2d-81fd-33f7083b3b9a"), ProductId = 13, Category = "AI", Name = "AI Builder Capacity", PurchaseUnit = "1M", Price = 10.99m } },
                { new Guid("94e2646a-4380-4cc1-bf36-b7254d2111cc"), new CCPProduct { SKU = new Guid("94e2646a-4380-4cc1-bf36-b7254d2111cc"), ProductId = 14, Category = "Productivity", Name = "Office 365", PurchaseUnit = "1M", Price = 12.99m } },
                { new Guid("a3a674cc-79cd-46d6-8af1-663fe078a684"), new CCPProduct { SKU = new Guid("a3a674cc-79cd-46d6-8af1-663fe078a684"), ProductId = 15, Category = "Productivity", Name = "Microsoft 365 E3", PurchaseUnit = "1M", Price = 32.99m } },
                { new Guid("97988609-90a8-4c54-95fd-fee9e87da2c0"), new CCPProduct { SKU = new Guid("97988609-90a8-4c54-95fd-fee9e87da2c0"), ProductId = 16, Category = "Analytics", Name = "Power BI Pro", PurchaseUnit = "1M", Price = 9.99m } },
                { new Guid("146046ca-2017-414b-bcda-0f02996dbbe7"), new CCPProduct { SKU = new Guid("146046ca-2017-414b-bcda-0f02996dbbe7"), ProductId = 17, Category = "AI", Name = "AI Builder Capacity", PurchaseUnit = "1M", Price = 10.99m } },
                { new Guid("d05abc3f-a6af-4191-91d2-a8af058dbb3b"), new CCPProduct { SKU = new Guid("d05abc3f-a6af-4191-91d2-a8af058dbb3b"), ProductId = 18, Category = "Productivity", Name = "Office 365", PurchaseUnit = "1M", Price = 12.99m } },
                { new Guid("f8ff3a76-c733-4728-aa36-f1e2e7a4d264"), new CCPProduct { SKU = new Guid("f8ff3a76-c733-4728-aa36-f1e2e7a4d264"), ProductId = 19, Category = "Productivity", Name = "Microsoft 365 E3", PurchaseUnit = "1M", Price = 32.99m } },
                { new Guid("71087d7e-6f20-40e4-be10-d2487c71f9e1"), new CCPProduct { SKU = new Guid("71087d7e-6f20-40e4-be10-d2487c71f9e1"), ProductId = 20, Category = "Analytics", Name = "Power BI Pro", PurchaseUnit = "1M", Price = 9.99m } }
            };
        }

        public async Task<AccountResponse> CreateAccountAsync(Account account)
        {
            return await Task.FromResult(new AccountResponse { AccountEmail = "MyFantasticSchool@oncloudcomputingprovider.com", Status = "Success" });
        } 

        public async Task<AccountResponse> DeleteAccountAsync(string accountEmail)
        {
            return await Task.FromResult(new AccountResponse { Status = "Success" });
        }

        public async Task<SubscriptionResponse> OrderSubscriptionAsync(string accountEmail, Guid productId, int amount)
        {
            var product = await GetProductBySKUAsync(productId);
            return await Task.FromResult(new SubscriptionResponse { SubstrictionId = "123456", Status = "Success", Product = product });
        }

        public Task<SubscriptionResponse> UpdateSubscriptionQuantityAsync(string accountEmail, Guid productId, int newQuantity)
        {
            return Task.FromResult(new SubscriptionResponse { Status = "Success" });
        }

        public Task<SubscriptionResponse> CancelSubscriptionAsync(string accountEmail, string productId)
        {
            return Task.FromResult(new SubscriptionResponse { Status = "Success" });
        }

        public Task<SubscriptionResponse> ExtendSubscriptionLicence(string accountEmail, string productId, DateTime newDate)
        {
            return Task.FromResult(new SubscriptionResponse { Status = "Success" });
        }
    }
}
