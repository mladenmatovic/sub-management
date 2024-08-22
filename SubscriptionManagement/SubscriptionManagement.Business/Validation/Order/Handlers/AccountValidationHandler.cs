using SubscriptionManagement.Business.Common;
using SubscriptionManagement.Business.Repositories;
using SubscriptionManagement.Business.Validation.Abstract;


namespace SubscriptionManagement.Business.Validation.Order.Handlers
{
    public class AccountValidationHandler : ValidationHandler<OrderContext>
    {
        private readonly IAccountRepository _accountRepository;

        public AccountValidationHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public override async Task<Result<bool, Error>> Validate(OrderContext context)
        {
            var account = await _accountRepository.GetByIdAsync(context.AccountId);
            if (account == null)
            {
                return new Error("OrderAdd.BadAccount", $"Failed to create subscription for non existent accountId: {context.AccountId}");
            }
            context.Account = account;
            return _nextHandler != null ? await _nextHandler.Validate(context) : true;
        }
    }
}
