namespace MinRobot.Models;

public class RobotStatus
{
    public string RobotId { get; set; } = default!;
    public string Status { get; set;} = default!;
    public DateTime LastUpdated { get; set;}
}