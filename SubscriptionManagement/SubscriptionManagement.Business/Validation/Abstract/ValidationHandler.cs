using SubscriptionManagement.Business.Common;

namespace SubscriptionManagement.Business.Validation.Abstract
{
    public abstract class ValidationHandler<TContext>
    {
        protected ValidationHandler<TContext> _nextHandler;

        public void SetNext(ValidationHandler<TContext> handler)
        {
            _nextHandler = handler;
        }

        public abstract Task<Result<bool, Error>> Validate(TContext context);
    }
}
