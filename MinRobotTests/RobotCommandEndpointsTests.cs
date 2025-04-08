//using Microsoft.AspNetCore.Mvc;
//using MinRobot.Application.Dto;
//using MinRobot.Application.Endpoints;
//using System.Net;
//using System.Threading;

//namespace MinRobot.Tests;

//[Fact]
//public async Task PostRobotCommandAsync_InvalidRobotId_ReturnsBadRequest()
//{
//    // Arrange
//    var commandDto = new RobotCommandDto
//    {
//        RobotId = "123", // Invalid format
//        CommandType = "MoveForward",
//        CommandData = "Distance:10",
//        Status = "Pending"
//    };

//    // Act
//    var result = await RobotCommandEndpoints.PostRobotCommandAsync(commandDto, _mockDatabaseService.Object, _cancellationToken);

//    // Assert
//    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//    var response = Assert.IsType<RobotCommandResponse<RobotCommand>>(badRequestResult.Value);
//    Assert.False(response.IsSuccess);
//    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
//    Assert.Contains("Invalid RobotId format", response.ErrorMessages);
//}

//[Fact]
//public async Task PostRobotCommandAsync_ValidRobotId_ReturnsCreated()
//{
//    // Arrange
//    var commandDto = new RobotCommandDto
//    {
//        RobotId = "TX-123", // Valid format
//        CommandType = "MoveForward",
//        CommandData = "Distance:10",
//        Status = "Pending"
//    };

//    _mockDatabaseService
//    .Setup(db => db.AddRobotCommandAsync(It.IsAny<RobotCommand>(), _cancellationToken))
//        .ReturnsAsync(1);

//    // Act
//    var result = await RobotCommandEndpoints.PostRobotCommandAsync(commandDto, _mockDatabaseService.Object, _cancellationToken);

//    // Assert
//    var createdResult = Assert.IsType<CreatedResult>(result);
//    var response = Assert.IsType<RobotCommandResponse<RobotCommand>>(createdResult.Value);
//    Assert.True(response.IsSuccess);
//    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
//}
