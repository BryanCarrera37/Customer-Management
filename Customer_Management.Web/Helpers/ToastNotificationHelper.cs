using Customer_Management.Web.Classes.Exception;
using NToastNotify;

namespace Customer_Management.Web.Helpers
{
    public static class ToastNotificationHelper
    {
        public static void ShowToastWhenValuesFormAreIncorrect(IToastNotification toast)
        {
            toast.AddWarningToastMessage("Los valores ingresados NO son correctos");
        }

        public static void HandleExceptionAndShowToast(IToastNotification toast, CustomHttpException exception)
        {
            var message = (exception.Value as CustomHttpExceptionBody)!.Message;
            if (IsMessageForWarningToast(exception))
            {
                ShowWarningToast(toast, message);
                return;
            }

            if (IsMessageForErrorToast(exception))
            {
                ShowErrorToast(toast, message);
                return;
            }

            throw new Exception("The status code presented in the Exception Instance is not defined");
        }

        private static bool IsMessageForWarningToast(CustomHttpException exception)
        {
            return exception.StatusCode >= StatusCodes.Status400BadRequest && exception.StatusCode < StatusCodes.Status500InternalServerError;
        }

        private static bool IsMessageForErrorToast(CustomHttpException exception)
        {
            return exception.StatusCode >= StatusCodes.Status500InternalServerError;
        }

        private static void ShowWarningToast(IToastNotification toast, string message)
        {
            toast.AddWarningToastMessage(message);
        }

        private static void ShowErrorToast(IToastNotification toast, string message)
        {
            toast.AddErrorToastMessage(message);
        }
    }
}
