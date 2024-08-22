namespace SubscriptionManagement.Business.Common
{
    public class Result<TValue, TError>
    {
        private readonly bool _isSuccess;
        public TValue Value { get; private set; }
        public TError Error { get; private set; }

        private Result(TValue value)
        {
            _isSuccess = true;
            Value = value;
            Error = default!;
        }

        private Result(TError error)
        {
            _isSuccess = false;
            Value = default!;
            Error = error;
        }

        public static implicit operator Result<TValue, TError>(TValue value) => new Result<TValue, TError>(value);

        public static implicit operator Result<TValue, TError>(TError error) => new Result<TValue, TError>(error);

        public TResult Match<TResult>(Func<TValue, TResult> success, Func<TError, TResult> failure)
         => _isSuccess ? success(Value) : failure(Error);
    }

    public record None();
}
