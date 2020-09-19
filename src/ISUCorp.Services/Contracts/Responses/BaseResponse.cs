namespace ISUCorp.Services.Contracts.Responses
{
    /// <summary>
    /// Represents an abstract response.
    /// </summary>
    public abstract class BaseResponse
    {
        /// <summary>
        /// Gets whether the response is successful.
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// Gets the response message.
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Creates a response.
        /// </summary>
        /// <param name="success">Whether the response is successful.</param>
        /// <param name="message">Response message.</param>
        public BaseResponse(bool success = false, string message = null)
        {
            Success = success;
            ErrorMessage = message;
        }
    }
}
