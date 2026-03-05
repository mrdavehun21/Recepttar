namespace Recepttar.Server.BLL.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string SuccessMessage { get; }
        public string ErrorMessage { get; }

        private Result(bool isSuccess, string? successMessage, string? errorMessage)
        {
            IsSuccess = isSuccess;
            SuccessMessage = successMessage;
            ErrorMessage = errorMessage;
        }

        public static Result Success(string message)
        {
            return new Result(true, message, null);
        }

        public static Result Failure(string error)
        {
            return new Result(false, null, error); 
        }
    }
}
