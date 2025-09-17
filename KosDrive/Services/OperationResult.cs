namespace KosDrive.Services
{
    public class OperationResult<T>
    {
        public T? Data { get; set; }
        public bool Successeded { get; set; }
        public string? Message { get; set; }
        public int Status { get; set; }

        public static OperationResult<T> Success(T data, string? message = null, int status = 200)
        {
            return new OperationResult<T>
            {
                Data = data,
                Successeded = true,
                Message = message,
                Status = status
            };
        }

        public static OperationResult<T> Failure(string message, int status = 400)
        {
            return new OperationResult<T>
            {
                Data = default,
                Successeded = false,
                Message = message,
                Status = status
            };
        }
    }
}
