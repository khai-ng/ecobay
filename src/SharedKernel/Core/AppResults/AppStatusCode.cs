namespace Core.AppResults
{
    public enum AppStatusCode
    {
        /// <summary>
        /// Indicates that the request has succeeded
        /// </summary>
        Ok,
        /// <summary>
        /// Indicates that the server has occur unexpected error 
        /// </summary>
        Error,
        /// <summary>
        /// Indicates that the server understood the request but refuses to authorize
        /// </summary>
        Forbidden,
        /// <summary>
        /// Indicates that the request requires authorization
        /// </summary>
        Unauthorized,
        /// <summary>
        /// Indicates that the request could not be understood by the server
        /// </summary>
        Invalid,
        /// <summary>
        /// Indicates that the server did not find a current representation for the target resource
        /// </summary>
        NotFound,
        /// <summary>
        /// Indicates that the request could not be completed due to a conflict with the current state of the target resource
        /// </summary>
        Conflict,
        /// <summary>
        /// Indicates that the server is temporarily unavailable
        /// </summary>
        Unavailable
    }
}
