using Microsoft.Extensions.Logging;
using SubscriptionManagement.Business.Common;
using SubscriptionManagement.Business.Models;
using SubscriptionManagement.Business.Repositories;

namespace SubscriptionManagement.Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ILogger<AccountService> _logger;

        public AccountService(
            IAccountRepository accountRepository,
            ISubscriptionRepository subscriptionRepository,
            ILogger<AccountService> logger)
        {
            _accountRepository = accountRepository;
            _subscriptionRepository = subscriptionRepository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Account>> GetAllForCustomer(Guid customerId)
        {
            try
            {
                var customers = await _accountRepository.GetAllForCustomerAsync(customerId);
                _logger.LogInformation($"Getting all accounts for customer {customerId}");
                return customers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Getting all accounts for customer {customerId}");
                throw;
            }
        }

        public async Task<Result<List<Subscription>, Error>> GetAllSubscriptions(Guid accountId)
        {
            try
            {
                var subs = await _subscriptionRepository.GetAllForAccountAsync(accountId);
                _logger.LogInformation($"Getting all subs for account {accountId}");
                return subs.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Getting all subscriptions for account {accountId}");
                throw;
            }
        }
    }
}
