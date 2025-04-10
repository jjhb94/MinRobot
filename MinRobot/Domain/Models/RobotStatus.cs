using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MinRobot.Domain.Models;

public class RobotStatus
{
    [BsonId]
    [BsonElement("_id")] // Explicitly map _id
    public ObjectId Id { get; set; }

    [BsonElement("robotId")] // Explicitly map robotId
    public required string RobotId { get; set; }

    [BsonElement("status")] // Explicitly map status
    public string Status { get; set; }

    [BsonElement("batteryLevel")] // Explicitly map batteryLevel
    public int BatteryLevel { get; set; }

    [BsonElement("uptime")] // Explicitly map uptime
    public int Uptime { get; set; }

    [BsonElement("lastUpdated")] // Explicitly map lastUpdated
    public System.DateTime LastUpdated { get; set; }

    [BsonElement("positionX")] // Explicitly map positionX
    public decimal PositionX { get; set; }

    [BsonElement("positionY")] // Explicitly map positionY
    public decimal PositionY { get; set; }
}