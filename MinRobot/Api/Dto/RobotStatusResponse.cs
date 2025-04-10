using System.Net;

namespace MinRobot.Api.Dto;

public class RobotStatusResponse<T>
{
    public RobotStatusResponse()
    {
        ErrorMessages = new List<string>();
    }

    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// The data returned by the operation.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// The number of items in the response.
    /// </summary>
    public int ItemCount { get; set; } = 0;

    /// <summary>
    /// The HTTP status code of the response.
    /// </summary>
    public HttpStatusCode StatusCode { get; set; }

    /// <summary>
    /// A list of error messages, if any.
    /// </summary>
    public List<string> ErrorMessages { get; set; }
}