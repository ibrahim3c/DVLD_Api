namespace DVLD.Core.Helpers
{
    public class Result<T>
    {
        public T? Value { get; }
        public bool IsSuccess { get; }
        public List<string>? Errors { get; }

        private Result(T? value, bool isSuccess, List<string>? errors = null)
        {
            Value = value;
            IsSuccess = isSuccess;
            Errors = errors ?? new List<string>();
        }

        // Factory Method for Success
        public static Result<T> Success(T value) => new Result<T>(value, true);

        // Factory Method for Failure (No need to pass a value)
        public static Result<T> Failure(List<string> errors) => new Result<T>(default, false, errors);
    }
}
