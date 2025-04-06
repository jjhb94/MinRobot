namespace MinRobot.Domain.Models;

public class RobotStatus
{
    public string RobotId { get; set; } = default!;
    public string Status { get; set;} = default!;

    public double BatteryLevel {get; set;}
    public TimeSpan Uptime {get; set;}
    public DateTime LastUpdated { get; set;}

    public double RobotPositionX {get; set;}
    public double RobotPositionY {get; set;}
}