using System.Net;
using MinRobot.Application.Dto;
using MinRobot.Application.Utilities;
using MinRobot.Domain.Models;
using Xunit;

namespace MinRobotTests.Utilities;
public class EndpointUtilitiesTests
{
    [Fact]
    public void ValidateAndMapCommand_ShouldReturnBadRequest_WhenRequiredFieldsAreMissing()
    {
        // Arrange
        var commandDto = new RobotCommandDto
        {
            RobotId = null,
            CommandData = null,
            Status = null,
            CommandType = null
        };

        // Act
        var (command, result) = EndpointUtilities.ValidateAndMapCommand(null, commandDto);

        // Assert
        Assert.Null(command);
        Assert.NotNull(result);

        // Extract the response body from the IResult
        var response = result as Microsoft.AspNetCore.Http.HttpResults.BadRequest<RobotCommandResponse<string>>;
        Assert.NotNull(response);
        Assert.Contains("RobotId, CommandData, and Status are required.", response.Value.ErrorMessages);
    }


    [Fact]
    public void ValidateAndMapCommand_ShouldReturnBadRequest_WhenInvalidCommandType()
    {
        // Arrange
        var commandDto = new RobotCommandDto
        {
            RobotId = "TX-123",
            CommandData = "SomeData",
            Status = "Active",
            CommandType = "InvalidCommandType"
        };

        // Act
        var (command, result) = EndpointUtilities.ValidateAndMapCommand(1, commandDto);

        // Assert
        Assert.Null(command);
        Assert.NotNull(result);

        // Extract the response body from the IResult
        var response = result as Microsoft.AspNetCore.Http.HttpResults.BadRequest<RobotCommandResponse<string>>;
        Assert.NotNull(response);
        Assert.Contains("Invalid CommandType.", response.Value.ErrorMessages);
    }

    [Fact]
    public void ValidateAndMapCommand_ShouldReturnCommand_WhenValidInput()
    {
        // Arrange
        var commandDto = new RobotCommandDto
        {
            RobotId = "TX-123", // Valid RobotId
            CommandData = "SomeData", // Valid CommandData
            Status = "Completed", // Valid Status (must match CommandStatusEnum)
            CommandType = "Rotate(90)" // Valid CommandType (must match CommandTypeEnum)
        };

        // Act
        var (command, result) = EndpointUtilities.ValidateAndMapCommand(1, commandDto);

        // Assert
        Assert.NotNull(command); // Ensure command is not null
        Assert.Null(result); // Ensure no error result
        Assert.Equal("TX-123", command.RobotId); // Check RobotId
        Assert.Equal("Rotate", command.CommandType); // Check normalized CommandType
        Assert.Equal("Degrees:90", command.CommandData); // Check mapped CommandData
        Assert.Equal("Completed", command.Status); // Check mapped Status
    }



    [Fact]
    public void ValidateCommandId_ShouldReturnBadRequest_WhenInvalidCommandId()
    {
        // Arrange
        var invalidCommandId = "abc";

        // Act
        var result = EndpointUtilities.ValidateCommandId(invalidCommandId);

        // Assert
        Assert.NotNull(result);

        // Extract the response body from the IResult
        var response = result as Microsoft.AspNetCore.Http.HttpResults.BadRequest<RobotCommandResponse<string>>;
        Assert.NotNull(response);
        Assert.Contains("Invalid commandId. Must be a positive integer.", response.Value.ErrorMessages);
    }


    [Fact]
    public void ValidateCommandId_ShouldReturnNull_WhenValidCommandId()
    {
        // Arrange
        var validCommandId = "123";

        // Act
        var result = EndpointUtilities.ValidateCommandId(validCommandId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void IsValidRobotId_ShouldReturnTrue_WhenValidRobotId()
    {
        // Arrange
        var validRobotId = "TX-123";

        // Act
        var isValid = EndpointUtilities.IsValidRobotId(validRobotId);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void IsValidRobotId_ShouldReturnFalse_WhenInvalidRobotId()
    {
        // Arrange
        var invalidRobotId = "123-TX";

        // Act
        var isValid = EndpointUtilities.IsValidRobotId(invalidRobotId);

        // Assert
        Assert.False(isValid);
    }
}