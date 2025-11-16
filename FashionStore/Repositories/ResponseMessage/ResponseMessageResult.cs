namespace FashionStore.Repositories.ResponseMessage
{
    public class ResponseMessageResult
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; } = 200;
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }

        public ResponseMessageResult SetSuccess(string message = "Success", object? data = null)
        {
            Success = true;
            StatusCode = 200;
            Message = message;
            Data = data;
            return this;
        }

        public ResponseMessageResult SetFail(string message, int statusCode = 400)
        {
            Success = false;
            StatusCode = statusCode;
            Message = message;
            Data = null;
            return this;
        }

        public ResponseMessageResult SetCustom(bool success, string message, int statusCode, object? data = null)
        {
            Success = success;
            StatusCode = statusCode;
            Message = message;
            Data = data;
            return this;
        }
    }
}
