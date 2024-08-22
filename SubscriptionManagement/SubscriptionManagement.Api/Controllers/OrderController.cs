using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SubscriptionManagement.Business.Commands;
using SubscriptionManagement.Business.Services;
using System.Globalization;

namespace SubscriptionManagement.Api.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IAccountService _accountService;
        private readonly IProductCatalogService _productCatalogService;

        public OrderController(
             IOrderService orderService,
             IAccountService accountService,
             IProductCatalogService productCatalogService) : base()
        {
            _orderService = orderService;
            _accountService = accountService;
            _productCatalogService = productCatalogService;
        }

        [HttpPost]
        public async Task<IActionResult> OrderNew([FromBody] OrderCommand orderCommand)
        {
            var validationResponse = ValidateOrderCommand(orderCommand);
            if (validationResponse != null)
            {
                return validationResponse;
            }

            var result = await _orderService.OrderNew(orderCommand.AccountId, orderCommand.AccountEmail, orderCommand.ProductSKU, orderCommand.Amount, DateTime.ParseExact(orderCommand.ValidTo, "yyyyMMdd", CultureInfo.InvariantCulture));

            return result.Match<IActionResult>(
                success: _ => Ok(),
                failure: BadRequest);
        }

        [HttpPatch("quantity")]
        public async Task<IActionResult> ChangeOrderQuantity([FromBody] OrderChangeQuantityCommand orderCommand)
        {
            if (orderCommand.Amount < 1)
            {
                return BadRequest("Quantity of requested licence must be greater than 0.");
            }

            var result = await _orderService.UpdateQuatity(orderCommand.SubscriptionId, orderCommand.AccountEmail, orderCommand.ProductSKU, orderCommand.Amount);

            return result.Match<IActionResult>(
                success: _ => Ok(),
                failure: BadRequest);
        }

        [HttpPatch("validity")]
        public async Task<IActionResult> ExtendOrderValidityDate([FromBody] OrderExtendDateCommand orderCommand)
        {
            var validationResponse = ValidateOrderCommand(orderCommand);
            if (validationResponse != null)
            {
                return validationResponse;
            }

            var result = await _orderService.ExtendSubscription(
                orderCommand.SubscriptionId,
                orderCommand.AccountEmail, 
                orderCommand.ProductSKU,
                DateTime.ParseExact(orderCommand.ValidTo, "yyyyMMdd", CultureInfo.InvariantCulture));

            return result.Match<IActionResult>(
                success: _ => Ok(),
                failure: BadRequest);
        }

        [HttpPatch("cancel")]
        public async Task<IActionResult> CancelOrder([FromBody] OrderCancelCommand orderCommand)
        {
            var result = await _orderService.CancelSubscription(orderCommand.SubscriptionId, orderCommand.AccountEmail, orderCommand.ProductSKU);

            return result.Match<IActionResult>(
                success: _ => Ok(),
                failure: BadRequest);
        }

        private IActionResult? ValidateOrderCommand(OrderCommand orderCommand)
        {
            string expectedDateFormat = "yyyyMMdd";

            if (orderCommand.Amount < 1)
            {
                return BadRequest("Quantity of requested license must be greater than 0.");
            }

            if (!DateTime.TryParseExact(orderCommand.ValidTo, expectedDateFormat,
                                        CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                return BadRequest($"Invalid date format. Expected format is {expectedDateFormat}.");
            }

            if (parsedDate <= DateTime.Now)
            {
                return BadRequest("The date must be in the future.");
            }

            return null;
        }

        private IActionResult? ValidateOrderCommand(OrderExtendDateCommand orderCommand)
        {
            string expectedDateFormat = "yyyyMMdd";

            if (!DateTime.TryParseExact(orderCommand.ValidTo, expectedDateFormat,
                                        CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                return BadRequest($"Invalid date format. Expected format is {expectedDateFormat}.");
            }

            if (parsedDate <= DateTime.Now)
            {
                return BadRequest("The date must be in the future.");
            }

            return null;
        }

        private bool TryParseDate(string dateString, out DateTime result)
        {
            const string expectedDateFormat = "yyyyMMdd";
            return DateTime.TryParseExact(dateString, expectedDateFormat,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        }

        private DateTime ParseDate(string dateString)
        {
            TryParseDate(dateString, out var result);
            return result;
        }
    }
}
