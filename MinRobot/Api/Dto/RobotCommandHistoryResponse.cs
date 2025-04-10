namespace MinRobot.Api.Dto
{
    public class RobotCommandHistoryResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }
}
