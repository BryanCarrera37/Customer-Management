namespace Customer_Management.Web.Classes.Exception
{
    public class CustomHttpExceptionBody
    {
        public CustomHttpExceptionBody(string message) =>
            Message = message;

        public string Message { get; set; }
    }
}
