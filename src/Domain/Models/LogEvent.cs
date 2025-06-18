using Domain.Enums;

namespace Domain.Models
{
    public class LogEvent
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public LogEventType EventType { get; set; }
        public string Description { get; set; }
        public string ClientIp { get; set; }
        public string CalledEndpoint { get; set; }
        public string Email { get; set; }
        public string MethodType { get; set; }
    }
}
