using MinRobot.Infrastructure.Serialization;

namespace MinRobot.Domain.Models;

public class RobotHistory
{
    [BsonId]
     [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public int HistoryId { get; set; } // Primary key for history record
    public string RobotId { get; set; } = default!;
    // public int CommandId { get; set; } // Foreign key to RobotCommand (if applicable)
    [BsonSerializer(typeof(CommandTypeEnumConverter))]
    public CommandTypeEnum CommandType { get; set; } = default!;
    public string CommandData { get; set; } = default!;
    public string Status { get; set; } = default!; // Status of the command at this point in history
    public DateTime Timestamp { get; set; } // When this history record was created
    public string? ErrorMessage { get; set; } // Optional error message
    // Add other relevant properties as needed (e.g., Position, Battery Level, etc.)
}