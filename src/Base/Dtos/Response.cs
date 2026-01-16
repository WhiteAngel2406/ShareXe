namespace ShareXe.Base.Dtos
{
    /// <summary>
    /// Base class for API responses that provides a standard structure for communicating operation success and status messages.
    /// </summary>
    /// <remarks>
    /// This abstract class serves as the foundation for all response objects in the application,
    /// ensuring consistent response handling across the API. Derived classes should extend this
    /// to include additional response-specific data.
    /// </remarks>
    public abstract class Response
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}

