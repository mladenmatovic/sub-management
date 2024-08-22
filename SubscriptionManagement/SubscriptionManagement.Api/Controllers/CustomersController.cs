using Microsoft.AspNetCore.Mvc;
using SubscriptionManagement.Business.Mapper;
using SubscriptionManagement.Business.Services;

namespace SubscriptionManagement.Api.Controllers
{
    public class CustomersController : BaseController
    {
        private readonly ICustomerService _customerService;

        public CustomersController(
             ICustomerService customerService) : base()
        {
            _customerService = customerService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _customerService.GetCustomers();

            if (result is null)
            {
                return StatusCode(403);
            }

            return Ok(result.Select(c=>c.ToDto()));
        }
    }
}
