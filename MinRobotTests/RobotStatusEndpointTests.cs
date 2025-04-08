using Xunit;
using Moq;
using MinRobot.Application.Endpoints;
using MinRobot.Application.Dto;
using MinRobot.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Net;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinRobot.Application;
using MinRobot.Domain.Interfaces;

namespace MinRobot.Tests;

public class RobotStatusEndpointTests
{
    private readonly Mock<IRobotStatusRepository> _mockStatusRepository;
    private readonly DatabaseService _databaseService;

    public RobotStatusEndpointTests()
    {
        _mockStatusRepository = new Mock<IRobotStatusRepository>();
        var mockCommandRepository = new Mock<IRobotCommandRepository>();
        var mockHistoryRepository = new Mock<IRobotHisotryRepository>();

        _databaseService = new DatabaseService(
            _mockStatusRepository.Object,
            mockCommandRepository.Object,
            mockHistoryRepository.Object
        );
    }

    [Fact]
    public async Task GetAllRobotStatusesAsync_ReturnsOk_WhenRobotsExist()
    {
        // Arrange
        var robots = new List<RobotStatus>
        {
            new RobotStatus { RobotId = "TX-010", Status = "online", BatteryLevel = 80 },
            new RobotStatus { RobotId = "TX-023", Status = "offline", BatteryLevel = 50 }
        };
        _mockStatusRepository.Setup(db => db.GetAllStatusesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(robots);

        // Act
        var result = await RobotStatusEndpoint.GetAllRobotStatusesAsync(_databaseService, CancellationToken.None);

        // Assert
        //var okResult = Assert.IsType<Microsoft.AspNetCore.Http.Results.OK<RobotStatusResponse<List<RobotStatus>>>>(result);
        var okResult = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<RobotStatusResponse<List<RobotStatus>>>>(result);
        Assert.True(okResult.Value.IsSuccess);
        Assert.Equal(robots.Count, okResult.Value.ItemCount);
    }

    [Fact]
    public async Task GetRobotStatusByIdAsync_ReturnsNotFound_WhenRobotDoesNotExist()
    {
        // Arrange
        _mockStatusRepository.Setup(db => db.GetRobotStatusByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((RobotStatus)null);

        // Act
        var result = await RobotStatusEndpoint.GetRobotStatusByIdAsync("INVALID_ID", _databaseService, CancellationToken.None);

        // Assert
        var notFoundResult = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NotFound<RobotStatusResponse<RobotStatus>>>(result);
        Assert.False(notFoundResult.Value.IsSuccess);
    }
}
