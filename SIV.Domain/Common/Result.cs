namespace SIV.Domain.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string ErrorMessage { get; }
        public int StatusCode { get; }

        protected Result(bool isSuccess, string errorMessage, int statusCode)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }

        public static Result Success() => new Result(true, string.Empty, 200);
        public static Result Failure(string errorMessage, int statusCode = 400) => new Result(false, errorMessage, statusCode);
    }

    public class Result<T> : Result
    {
        public T Value { get; }

        private Result(T value, bool isSuccess, string errorMessage, int statusCode)
            : base(isSuccess, errorMessage, statusCode)
        {
            Value = value;
        }

        public static Result<T> Success(T value) => new Result<T>(value, true, string.Empty, 200);

        public static new Result<T> Failure(string errorMessage, int statusCode = 400)
            => new Result<T>(default!, false, errorMessage, statusCode);
    }
}