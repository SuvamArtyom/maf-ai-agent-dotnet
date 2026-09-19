namespace AIAgent.WebApi.DomainLayer.Modules.Common
{
    /// <summary>
    /// Standard application error representation for API responses.
    /// </summary>
    public class AppError
    {
        /// <summary>
        /// HTTP status code associated with the error.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Human-readable error message.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Detailed list of validation or execution errors, if applicable.
        /// </summary>
        public List<string> Details { get; set; } = new List<string>();

        /// <summary>
        /// Timestamp in UTC when the error was captured.
        /// </summary>
        public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Initializes a new instance of <see cref="AppError"/>.
        /// </summary>
        public AppError() { }

        /// <summary>
        /// Initializes a new instance of <see cref="AppError"/> with specific status code and message.
        /// </summary>
        public AppError(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="AppError"/> with specific status code, message, and details list.
        /// </summary>
        public AppError(int statusCode, string message, IEnumerable<string> details)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details.ToList();
        }
    }
}

