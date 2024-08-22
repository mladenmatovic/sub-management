using SubscriptionManagement.Business.Common;
using SubscriptionManagement.Business.Repositories;
using SubscriptionManagement.Business.Validation.Abstract;

namespace SubscriptionManagement.Business.Validation.Order.Handlers
{
    public class ProductValidationHandler : ValidationHandler<OrderContext>
    {
        private readonly ICloudPartnerRepository _cloudPartnerRepository;

        public ProductValidationHandler(ICloudPartnerRepository cloudPartnerRepository)
        {
            _cloudPartnerRepository = cloudPartnerRepository;
        }

        public override async Task<Result<bool, Error>> Validate(OrderContext context)
        {
            var product = await _cloudPartnerRepository.GetProductBySKUAsync(context.ProductSKU);
            if (product == null)
            {
                return new Error("OrderAdd.BadProduct", $"Failed to create subscription for non existent product SKU: {context.ProductSKU}");
            }
            context.Product = product;
            return _nextHandler != null ? await _nextHandler.Validate(context) : true;
        }
    }
}
