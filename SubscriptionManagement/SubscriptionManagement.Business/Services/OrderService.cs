using Microsoft.Extensions.Logging;
using SubscriptionManagement.Business.Common;
using SubscriptionManagement.Business.Models;
using SubscriptionManagement.Business.Models.CCP;
using SubscriptionManagement.Business.Repositories;
using SubscriptionManagement.Business.Validation.Order;
using SubscriptionManagement.Business.Validation.Order.Handlers;

namespace SubscriptionManagement.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ICloudPartnerRepository _cloudPartnerRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IAccountRepository accountRepository,
            ISubscriptionRepository subscriptionRepository,
            ICloudPartnerRepository cloudPartnerRepository,
            ILogger<OrderService> logger)
        {
            _accountRepository = accountRepository;
            _subscriptionRepository = subscriptionRepository;
            _cloudPartnerRepository = cloudPartnerRepository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<bool, Error>> OrderNew(Guid accountId, string email, Guid productSKU, int amount, DateTime validTo)
        {
            var context = new OrderContext
            {
                AccountId = accountId,
                Email = email,
                ProductSKU = productSKU,
                Amount = amount,
                ValidTo = validTo
            };
            var accountHandler = new AccountValidationHandler(_accountRepository);
            var productHandler = new ProductValidationHandler(_cloudPartnerRepository);

            accountHandler.SetNext(productHandler);

            var validationResult = await accountHandler.Validate(context);             
            if (!validationResult.Match(success => true, error => false))
            {
                return validationResult;
            }

            var ccpRespose = await _cloudPartnerRepository.OrderSubscriptionAsync(email, productSKU, amount);

            if (ccpRespose.Status != Status.SUCCESS)
            {
                var message = $"Failed to create subscription in Cloud Partner API. Email: {email}, Product: {productSKU}, Amount: {amount}";
                _logger.LogError(message);
                return new Error("OrderAdd.CloudAPIFail", message);
            }            

            try
            {
                var subscription = new Subscription
                {
                    AccountId = accountId,
                    ProductId = productSKU.ToString(),
                    Quantity = amount,
                    ValidTo = validTo,
                    ProductName = ccpRespose.Product?.Name,
                    PurchaseUnit = ccpRespose.Product?.PurchaseUnit,
                    Status = Enums.SubscriptionState.Active,
                };

                await _subscriptionRepository.Add(subscription);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to add subscription in system. Email: {email}, Product: {productSKU}. Error {ex.Message}");

                string message = string.Empty;
                try
                {
                    await _cloudPartnerRepository.CancelSubscriptionAsync(email, productSKU.ToString());
                    message = $"Failed to create a subscription locally. Whole operation failed.";
                }
                catch (Exception cancelException)
                {
                    var addedMessage = $"Failed to rollback ordering new subscription in Cloud Partner API. Email: {email}, Product: {productSKU}";
                    _logger.LogError(cancelException, addedMessage);
                    message = addedMessage;
                }

                return new Error("OrderAdd.CloudAPIFailedRollback", message);
            }
        }

        public async Task<Result<bool, Error>> ExtendSubscription(Guid subscriptionId, string email, Guid productSKU, DateTime newDate)
        {
            var context = new OrderContext
            {
                SubscriptionId = subscriptionId,
                ProductSKU = productSKU,
            };
            var subscriptionHandler = new SubscriptionValidationHandler(_subscriptionRepository);
            var productHandler = new ProductValidationHandler(_cloudPartnerRepository);

            subscriptionHandler.SetNext(productHandler);

            var validationResult = await subscriptionHandler.Validate(context);
            if (!validationResult.Match(success => true, error => false))
            {
                return validationResult;
            }

            try
            {
                var cppResponse = await _cloudPartnerRepository.ExtendSubscriptionLicence(email, productSKU.ToString(), newDate);

                if (cppResponse.Status != Status.SUCCESS)
                {
                    var message = $"Failed to extend subscription in Cloud Partner API. Account: {email}, Product: {productSKU}, New Valid To: {newDate}";
                    _logger.LogError(message);
                    return new Error("OrderAdd.CloudAPIFail", message);
                }

                var subscription = await _subscriptionRepository.GetByIdAsync(subscriptionId);
                subscription.ValidTo = newDate;

                await _subscriptionRepository.Update(subscription);

                return true;
            }
            catch (Exception ex)
            {                
                _logger.LogError(ex, $"Failed to extend subscription. Account: {email}, Product: {productSKU}, New Valid To: {newDate}");

                // If the subscription was extended in the Cloud Partner API, attempt to roll back the change
                string message = string.Empty;
                try
                {                    
                    await _cloudPartnerRepository.CancelSubscriptionAsync(email, productSKU.ToString());
                    message = $"Failed to create a subscription locally. Whole operation failed.";
                }
                catch (Exception cancelException)
                {
                    var addedMessage = $"Failed to rollback ordering new subscription in Cloud Partner API. Email: {email}, Product: {productSKU}";
                    _logger.LogError(cancelException, addedMessage);
                    message = addedMessage;
                }

                return new Error("OrderAdd.CloudAPIFailedRollback", message);
            }
        }

        public async Task<Result<bool, Error>> UpdateQuatity(Guid subscriptionId, string email, Guid productSKU, int newAmount)
        {
            var context = new OrderContext
            {
                SubscriptionId = subscriptionId,
                ProductSKU = productSKU,
            };
            var subscriptionHandler = new SubscriptionValidationHandler(_subscriptionRepository);
            var productHandler = new ProductValidationHandler(_cloudPartnerRepository);

            subscriptionHandler.SetNext(productHandler);

            var validationResult = await subscriptionHandler.Validate(context);
            if (!validationResult.Match(success => true, error => false))
            {
                return validationResult;
            }

            int remedyAmount = -1;

            try
            {
                var cppResponse = await _cloudPartnerRepository.UpdateSubscriptionQuantityAsync(email, productSKU, newAmount);

                if (cppResponse.Status != Status.SUCCESS)
                {
                    _logger.LogError($"Failed to update subscription in Cloud Partner API. Account: {email}, Product: {productSKU}, New Quantity: {newAmount}");
                    return false;
                }

                var subscription = await _subscriptionRepository.GetByIdAsync(subscriptionId);
                remedyAmount = subscription.Quantity;
                subscription.Quantity = newAmount;
                await _subscriptionRepository.Update(subscription);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update subscription. Account: {email}, Product: {productSKU}, New Quantity: {newAmount}");

                string message = string.Empty;
                try
                {
                    await _cloudPartnerRepository.UpdateSubscriptionQuantityAsync(email, productSKU, remedyAmount);
                    message = $"Failed to create a subscription locally. Whole operation failed.";
                }
                catch (Exception cancelException)
                {
                    var addedMessage = $"Failed to rollback ordering new subscription in Cloud Partner API. Email: {email}, Product: {productSKU}";
                    _logger.LogError(cancelException, addedMessage);
                    message = addedMessage;
                }

                return new Error("OrderAdd.CloudAPIFailedRollback", message);
            }
        }

        public async Task<Result<bool, Error>> CancelSubscription(Guid subscriptionId, string email, Guid productSKU)
        {
            var context = new OrderContext
            {
                SubscriptionId = subscriptionId,
                ProductSKU = productSKU,
            };
            var subscriptionHandler = new SubscriptionValidationHandler(_subscriptionRepository);
            var productHandler = new ProductValidationHandler(_cloudPartnerRepository);

            subscriptionHandler.SetNext(productHandler);

            var validationResult = await subscriptionHandler.Validate(context);
            if (!validationResult.Match(success => true, error => false))
            {
                return validationResult;
            }

            var remedyDate = DateTime.MinValue;

            try
            {                
                var cppResponse = await _cloudPartnerRepository.CancelSubscriptionAsync(email, productSKU.ToString());

                if (cppResponse.Status != Status.SUCCESS)
                {
                    _logger.LogError($"Failed to cancel subscription in Cloud Partner API. Account: {email}, Product: {productSKU}");
                    return false;
                }

                var subscription = await _subscriptionRepository.GetByIdAsync(subscriptionId);
                // in case of the whole transaction fails, to have date to rollback change
                remedyDate = subscription.ValidTo; 
                subscription.ValidTo = DateTime.UtcNow;
                subscription.Status = Enums.SubscriptionState.Cancelled;

                await _subscriptionRepository.Update(subscription);

                return true;
            }
            catch (Exception ex)
            {                
                _logger.LogError(ex, $"Failed to cancel subscription. Account: {email}, Product: {productSKU}");

                string message = string.Empty;
                // If the subscription was cancelled in the Cloud Partner API, attempt to re-create it
                try
                {
                    await _cloudPartnerRepository.ExtendSubscriptionLicence(email, productSKU.ToString(), remedyDate);
                    message = $"Failed to create a subscription locally. Whole operation failed.";
                }
                catch (Exception cancelException)
                {
                    var addedMessage = $"Failed to rollback ordering new subscription in Cloud Partner API. Email: {email}, Product: {productSKU}";
                    _logger.LogError(cancelException, addedMessage);
                    message = addedMessage;
                }

                return new Error("OrderAdd.CloudAPIFailedRollback", message);
            }
        }        
    }
}
