namespace MinRobot.Domain.Models;

public enum CommandTypeEnum
{
    MoveForward, 
    MoveBackward,
    TurnLeft,
    TurnRight,
    Stop,
    StartCharging,
    StopCharging,
    PickUpItem,
    DropItem,
    ScanArea,
    ReportStatus,
    Rotate
}