using System;
using System.Globalization;
using MinRobot.Application.Utilities;
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
                CommandData = "x:10.5,y:20.3"
            };
            var robotStatus = new RobotStatus();

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
                CommandData = "x:abc,y:def"
            };
            var robotStatus = new RobotStatus();

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
                CommandData = "x:10.5" // Invalid format: Missing the y-coordinate
            };
            var robotStatus = new RobotStatus();

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
                CommandData = null
            };
            var robotStatus = new RobotStatus
            {
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
