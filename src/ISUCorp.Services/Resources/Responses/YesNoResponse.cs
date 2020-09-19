using ISUCorp.Services.Contracts.Responses;

namespace ISUCorp.Services.Resources.Responses
{
    public class YesNoResponse : BaseResponse
    {
        /// <summary>
        /// Creates a success response.
        /// </summary>
        public YesNoResponse() : base(true, "Success")
        {
        }

        /// <summary>
        /// Creates an failed response.
        /// </summary>
        /// <param name="message">Error message related to the failed response.</param>
        public YesNoResponse(string message) : base(false, message)
        {
        }
    }
}
