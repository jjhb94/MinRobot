
namespace MinRobot.Domain.Models;

public class RobotCommand
{
    [BsonId]
     [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    // public int CommandId { get; set; } // Database is integer.
    public string RobotId { get; set; } = default!;
    public required string CommandType { get; set; } // tried using CommandTypeEnum but it was causing issues with serialization.
    // maybe we can store enums as INT in the db column for more efficient storage and querying.
    public string CommandData { get; set; } = default!;
    public double? Degrees {get; set;} // Nullable; not all commands have degrees
    public DateTime CreatedAt { get; set; }
    // public DateTime? ExecutedAt { get; set; }
    // public string Status { get; set; } = default!;
    public string Status {get; set;} = default!;
    public string? ErrorMessage { get; set; } // Nullable to allow for no error message.
}