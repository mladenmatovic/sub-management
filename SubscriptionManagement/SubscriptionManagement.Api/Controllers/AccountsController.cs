using Microsoft.AspNetCore.Mvc;
using SubscriptionManagement.Business.Mapper;
using SubscriptionManagement.Business.Services;

namespace SubscriptionManagement.Api.Controllers
{
    public class AccountsController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IProductCatalogService _productCatalogService;

        public AccountsController(
             IAccountService accountService,
             IProductCatalogService productCatalogService) : base()
        {
            _accountService = accountService;
            _productCatalogService = productCatalogService;
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> Get(Guid customerId)
        {
            var result = await _accountService.GetAllForCustomer(customerId);

            if (result is null)
            {
                return StatusCode(403);
            }

            return Ok(result.Select(a => a.ToDto()));
        }

        [HttpGet("product-catalog/{accountId}")]
        public async Task<IActionResult> GetProductCatalog(Guid accountId)
        {
            var result = await _productCatalogService.GetAllForAccount(accountId);

            if (result is null)
            {
                return StatusCode(403);
            }

            return Ok(result);
        }


        [HttpGet("{accountId}/subscriptions")]
        public async Task<IActionResult> GetSubscriptions(Guid accountId)
        {
            var result = await _accountService.GetAllSubscriptions(accountId);

            return result.Match<IActionResult>(
                success: _ => Ok(result.Value.Select(p=>p.ToDto())),
                failure: BadRequest);
        }
    }
}
