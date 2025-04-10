using System.Net;

namespace MinRobot.Api.Dto;

public class RobotCommandResponse<T>
{
    public RobotCommandResponse()
    {
        ErrorMessages = new List<string>();
    }
    
    public bool IsSuccess { get; set; }
    public T? Data { get; set; } // generic type for the response data object
    public int ItemCount { get; set; } = 0;
    public HttpStatusCode StatusCode { get; set; }
    public List<string> ErrorMessages { get; set; }
}