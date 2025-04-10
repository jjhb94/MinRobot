using MongoDB.Bson;
using MinRobot.Domain.Models;
namespace MinRobot.Application.Dto;
public class RobotCommandHistoryDto
{
    public string CommandId { get; set; }
    public string CommandType { get; set; } = default!;
    public string CommandData { get; set; } = default!;
}