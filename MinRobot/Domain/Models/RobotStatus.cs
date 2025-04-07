namespace MinRobot.Domain.Models;

public class RobotStatus
{
    public string RobotId { get; set; } = default!;
    public string Status { get; set; } = default!;

    public int BatteryLevel { get; set; } // Database is integer.
    public TimeSpan Uptime { get; set; }
    public DateTime LastUpdated { get; set; }

    public decimal PositionX { get; set; } // Database is numeric.
    public decimal PositionY { get; set; } // Database is numeric.
}