namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

/// <summary>
///     Unified interface for error handling in use cases.
/// </summary>
public interface IErrorOutputPort
{
    /// <summary>
    ///     Informs that the resource was not found.
    /// </summary>
    /// <param name="message">Description of the error.</param>
    void NotFoundHandle(string message);

    /// <summary>
    ///     Informs that the request was not correct (validation errors, domain constraints).
    /// </summary>
    /// <param name="message">Description of the error.</param>
    void BadRequestHandle(string message);

    /// <summary>
    ///     Informs that an unexpected error occurred.
    /// </summary>
    /// <param name="message">Description of the error.</param>
    void GeneralErrorHandle(string message);
}
