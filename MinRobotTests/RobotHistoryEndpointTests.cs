// using Xunit;
// using Moq;
// using MinRobot.Api.Endpoints;
// using MinRobot.Api.Dto;
// using MinRobot.Domain.Models;
// using Microsoft.AspNetCore.Http;
// using System.Net;
// using System.Threading;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using MinRobot.Api;

// namespace MinRobot.Tests
// {
//     public class RobotHistoryEndpointTests
//     {
//         private readonly Mock<DatabaseService> _mockDatabaseService;

//         public RobotHistoryEndpointTests()
//         {
//             _mockDatabaseService = new Mock<DatabaseService>(null, null, null);
//         }

//         [Fact]
//         public async Task GetRobotHistoryAsync_ReturnsOk_WhenHistoryExists()
//         {
//             // Arrange
//             var history = new List<RobotCommandHistoryDto>
//             {
//                 new RobotCommandHistoryDto { CommandId = 1, CommandType = "MoveForward", CommandData = "10 meters" },
//                 new RobotCommandHistoryDto { CommandId = 2, CommandType = "Rotate", CommandData = "90 degrees" }
//             };
//             _mockDatabaseService.Setup(db => db.GetRobotCommandHistoryAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
//                 .ReturnsAsync(history);

//             // Act
//             var result = await RobotHistoryEndpoint.GetRobotHistoryAsync("TX-010", _mockDatabaseService.Object, CancellationToken.None);

//             // Assert
//             var okResult = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<RobotCommandHistoryResponse<IEnumerable<RobotCommandHistoryDto>>>>(result);
//             Assert.True(okResult.Value.IsSuccess);
//             Assert.Equal(history.Count, okResult.Value.Data.Count());
//         }

//         [Fact]
//         public async Task GetRobotHistoryAsync_ReturnsOk_WhenNoHistoryExists()
//         {
//             // Arrange
//             _mockDatabaseService.Setup(db => db.GetRobotCommandHistoryAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
//                 .ReturnsAsync(new List<RobotCommandHistoryDto>());

//             // Act
//             var result = await RobotHistoryEndpoint.GetRobotHistoryAsync("TX-010", _mockDatabaseService.Object, CancellationToken.None);

//             // Assert
//             var okResult = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<RobotCommandHistoryResponse<IEnumerable<RobotCommandHistoryDto>>>>(result);
//             Assert.True(okResult.Value.IsSuccess);
//             Assert.Null(okResult.Value.Data);
//         }
//     }
// }
