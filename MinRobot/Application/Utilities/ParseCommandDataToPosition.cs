// private (decimal X, decimal Y)? ParsePositionData(string commandData)
// {
//     var positionRegex = new Regex(@"x:(\d+),\s*y:(\d+)", RegexOptions.IgnoreCase);
//     var match = positionRegex.Match(commandData);

//     if (match.Success)
//     {
//         var x = decimal.Parse(match.Groups[1].Value);
//         var y = decimal.Parse(match.Groups[2].Value);
//         return (x, y);
//     }

//     return null; // No position data found
// }
