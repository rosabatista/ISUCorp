using ISUCorp.Services.Contracts.Responses;

namespace ISUCorp.Services.Resources.Responses
{
    /// <summary>
    /// Represents a response related to a data.
    /// </summary>
    /// <typeparam name="T">Data belonging to the response.</typeparam>
    public class DataResponse<T> : BaseResponse
    {
        /// <summary>
        /// Gets the data related to the response, if any.
        /// </summary>
        public T Data { get; private set; }

        public DataResponse() : base()
        { 
        
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="data">Data related to the response.</param>
        public DataResponse(T data) : base(true, string.Empty)
        {
            Data = data;
        }

        /// <summary>
        /// Creates a failed response.
        /// </summary>
        /// <param name="errorMessage">Error message related to the failed response.</param>
        public DataResponse(string errorMessage) : base(false, errorMessage)
        {
            Data = default;
        }
    }
}
