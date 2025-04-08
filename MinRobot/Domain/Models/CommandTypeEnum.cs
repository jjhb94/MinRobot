namespace MinRobot.Domain.Models;

public enum CommandTypeEnum
{
    MOVEFORWARD,
    MOVEBACKWARD,
    TURNLEFT,
    TURNRIGHT,
    STOP,
    STARTCHARGING,
    STOPCHARGING,
    PICKUPITEM,
    DROPITEM,
    SCANAREA,
    REPORTSTATUS
}