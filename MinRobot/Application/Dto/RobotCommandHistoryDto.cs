namespace MinRobot.Application.Dto;
public class RobotCommandHistoryDto
{
    public int CommandId { get; set; }
    public string CommandType { get; set; } = default!;
    public string CommandData { get; set; } = default!;
}