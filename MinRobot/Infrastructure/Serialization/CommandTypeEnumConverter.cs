using MongoDB.Bson.Serialization;

namespace MinRobot.Infrastructure.Serialization;

public class CommandTypeEnumConverter : IBsonSerializer<CommandTypeEnum>
{
    // The type of the value this converter handles
    public Type ValueType => typeof(CommandTypeEnum);

    // Deserialize method: Convert the BSON value into a CommandTypeEnum
    public CommandTypeEnum Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var bsonReader = context.Reader;
        var value = bsonReader.ReadString();
        return Enum.TryParse<CommandTypeEnum>(value, out var result) ? result : CommandTypeEnum.MoveForward; // Default to MoveForward if invalid
    }

    // Serialize method: Convert the CommandTypeEnum to a BSON value
    public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, CommandTypeEnum value)
    {
        var bsonWriter = context.Writer;
        bsonWriter.WriteString(value.ToString());
    }

    // Serialize method for object (boxed CommandTypeEnum)
    public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
    {
        if (value is CommandTypeEnum enumValue)
        {
            Serialize(context, args, enumValue); // Call the CommandTypeEnum specific serialize method.
        }
        else
        {
            throw new ArgumentException($"Value must be of type {typeof(CommandTypeEnum)}.", nameof(value));
        }
    }

    object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        return Deserialize(context, args);
    }
}