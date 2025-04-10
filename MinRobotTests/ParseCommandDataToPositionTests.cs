using System;
using System.Globalization;
using MinRobot.Api.Utilities;
using MinRobot.Domain.Models;
using Xunit;

namespace MinRobotTests.Utilities
{
    public class ParseCommandDataToPositionTests
    {
        [Fact]
        public void ProcessCommandAndUpdateStatus_ShouldUpdatePosition_WhenValidCommandData()
        {
            // Arrange
            var command = new RobotCommand
            {
                CommandType = "MoveRight",
                CommandData = "x:10.5,y:20.3",
                Status = "Active"

            };
            var robotStatus = new RobotStatus()
            {
                RobotId = "TX-123" // Required property
            };

            // Act
            var updatedStatus = ParseCommandDataToPosition.ProcessCommandAndUpdateStatus(command, robotStatus);

            // Assert
            Assert.Equal(10.5m, updatedStatus.PositionX);
            Assert.Equal(20.3m, updatedStatus.PositionY);
        }

        [Fact]
        public void ProcessCommandAndUpdateStatus_ShouldThrowException_WhenInvalidCoordinates()
        {
            // Arrange
            var command = new RobotCommand
            {
                CommandType = "PickUpItem",
                CommandData = "x:abc,y:def",
                Status = "Active"
            };
            var robotStatus = new RobotStatus { RobotId = "TX-123" }; // Required property}

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                ParseCommandDataToPosition.ProcessCommandAndUpdateStatus(command, robotStatus));
            Assert.Equal("Invalid coordinates in commandData.", exception.Message);
        }

        [Fact]
        public void ProcessCommandAndUpdateStatus_ShouldThrowException_WhenInvalidCommandDataFormat()
        {
            // Arrange
            var command = new RobotCommand
            {
                CommandType = "ReportStatus",
                CommandData = "x:10.5", // Invalid format: Missing the y-coordinate
                Status = "Active"
            };
            var robotStatus = new RobotStatus { RobotId = "TX-123" }; // Required property};

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                ParseCommandDataToPosition.ProcessCommandAndUpdateStatus(command, robotStatus));
            Assert.Equal("Invalid commandData format.", exception.Message);
        }



        [Fact]
        public void ProcessCommandAndUpdateStatus_ShouldNotUpdatePosition_WhenCommandDataIsNull()
        {
            // Arrange
            var command = new RobotCommand
            {
                CommandType = "Stop",
                CommandData = string.Empty,
                Status = "Active"
            };
            var robotStatus = new RobotStatus
            {
                RobotId = "TX-123", // Required property
                PositionX = 5.0m,
                PositionY = 10.0m
            };

            // Act
            var updatedStatus = ParseCommandDataToPosition.ProcessCommandAndUpdateStatus(command, robotStatus);

            // Assert
            Assert.Equal(5.0m, updatedStatus.PositionX);
            Assert.Equal(10.0m, updatedStatus.PositionY);
        }
    }
}
