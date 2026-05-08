using JetBrains.Annotations;
using PT.Tools.Debugging;

namespace PT.Tools.Http
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T Value { get; }
        public string Error { get; }

        private Result(T value)
        {
            IsSuccess = true;
            Value = value;
            Error = null;

            DebugManager.Log(DebugCategory.Backend, $"Result created for {value.GetType()} is successful");
        }

        private Result(string error)
        {
            IsSuccess = false;
            Error = error;
            Value = default;
            
            DebugManager.Log(DebugCategory.Backend, $"Result failed with error: {error}");
        }

        public static Result<T> Success(T value) => new(value);

        public static Result<T> Fail(string error) => new(error);
    }
}