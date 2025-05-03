namespace CommonSharedLibrary.Middleware.Constants
{
    public static class ExceptionConstants
    {
        // Error messages
        public const string InternalServerErrorMessage = "Sorry, an internal server error occurred. Kindly try again.";
        public const string TooManyRequestsMessage = "Too many requests made.";
        public const string UnauthorizedMessage = "You are not authorized to access.";
        public const string ForbiddenMessage = "You are not required to access.";
        public const string TimeoutMessage = "Request timeout... try again.";
        //Error titles
        public const string ErrorTitle = "Error";
        public const string TooManyRequestsTitle = "Warning";
        public const string UnauthorizedTitle = "Alert";
        public const string ForbiddenTitle = "Out of access";
        public const string TimeoutTitle = "Out of time";
    }
}

