// MinRobot.Application/Dto/RobotCommandDto.cs
namespace MinRobot.Application.Dto;

public class RobotCommandDto
{
    public string RobotId { get; set; } = default!;
    public string CommandType { get; set; } = default!;
    public string CommandData { get; set; } = default!;
}