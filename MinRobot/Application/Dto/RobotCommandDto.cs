using System.ComponentModel.DataAnnotations;

namespace MinRobot.Application.Dto;

public class RobotCommandDto
{
    [Required]
    [RegularExpression(@"^[A-Z]{2}-\d+$", ErrorMessage = "Invalid RobotId format. Expected format: 'TX-123'.")]
    public string RobotId { get; set; } = default!;
    public string CommandType { get; set; } = default!;
    public string CommandData { get; set; } = default!;
    public string Status { get; set; } = default!;
}