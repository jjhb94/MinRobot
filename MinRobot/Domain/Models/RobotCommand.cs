// namespace MinRobot.Domain.Models;

public class RobotCommand
{
    public int CommandId { get; set; } // Database is integer.
    public string RobotId { get; set; } = default!;
    public string CommandType { get; set; } = default!;
    public string CommandData { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    // public DateTime? ExecutedAt { get; set; }
    public string Status { get; set; } = default!;
    public string? ErrorMessage { get; set; } // Nullable to allow for no error message.
}