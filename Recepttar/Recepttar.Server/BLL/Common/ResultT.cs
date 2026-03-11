namespace Recepttar.Server.BLL.Common
{
    public class ResultT<T>
    {
        public bool IsSuccess { get; }
        public T Data { get; }
        public string SuccessMessage { get; }
        public string ErrorMessage { get; }

        private ResultT(bool isSuccess, T? data, string? successMessage, string? errorMessage)
        {
            IsSuccess = isSuccess;
            Data = data;
            SuccessMessage = successMessage;
            ErrorMessage = errorMessage;
        }

        public static ResultT<T> Success(T data, string message = "")
        {
            return new ResultT<T>(true, data, message, null);
        }

        public static ResultT<T> Failure(string error)
        {
            return new ResultT<T>(false, default, null, error);
        }
    }
}
