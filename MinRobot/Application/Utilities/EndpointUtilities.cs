
using System.Net;
using MinRobot.Application.Dto;

namespace MinRobot.Api.Utilities;

public static class EndpointUtilities
{
    public static IResult HandleException(string message, Exception ex)
    {
        return Results.Problem(new RobotCommandResponse<string>
        {
            IsSuccess = false,
            StatusCode = HttpStatusCode.InternalServerError,
            ErrorMessages = new List<string> { message, ex.ToString() }
        }.ErrorMessages.FirstOrDefault(), statusCode: (int)HttpStatusCode.InternalServerError);
    }
}