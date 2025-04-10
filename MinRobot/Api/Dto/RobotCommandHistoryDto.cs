namespace MinRobot.Api.Dto;
public class RobotCommandHistoryDto
{
    public required string CommandId { get; set; }
    public string CommandType { get; set; } = default!;
    public string CommandData { get; set; } = default!;
}