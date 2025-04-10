using System.Globalization;
using MinRobot.Domain.Models;

namespace MinRobot.Api.Utilities;
public static class ParseCommandDataToPosition
{

    public static RobotStatus ProcessCommandAndUpdateStatus(RobotCommand command, RobotStatus robotStatus)
    {
        if (command.CommandData != null && command.CommandData.StartsWith("x:"))
        {
            string[] coordinates = command.CommandData.Split(',');

            if (coordinates.Length == 2)
            {
                string xString = coordinates[0].Substring(2); // Remove "x:"
                string yString = coordinates[1].Substring(2); // Remove "y:"

                if (decimal.TryParse(xString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal x) &&
                    decimal.TryParse(yString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal y))
                {
                    robotStatus.PositionX = x;
                    robotStatus.PositionY = y;
                }
                else
                {

                    throw new ArgumentException("Invalid coordinates in commandData.");
                }
            }
            else
            {

                throw new ArgumentException("Invalid commandData format.");
            }
        }
        return robotStatus;
    }
}