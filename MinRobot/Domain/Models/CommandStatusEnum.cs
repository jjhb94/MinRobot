namespace MinRobot.Domain.Models;

public enum CommandStatusEnum // rename RobotCommandStatusEnum ?
{
    Pending,
    Executing,
    Completed,
    Failed,
    Cancelled
}