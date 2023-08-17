namespace Customer_Management.Web.Classes.Exception
{
    public class CustomHttpException : System.Exception
    {
        public CustomHttpException(int statusCode, object? value) =>
            (StatusCode, Value) = (statusCode, value);

        public int StatusCode { get; }
        public object? Value { get; }
    }
}
