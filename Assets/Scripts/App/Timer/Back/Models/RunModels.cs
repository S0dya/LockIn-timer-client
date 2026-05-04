using System.Collections.Generic;

namespace App.Timer.Back.Models
{
    public class SessionStartResponse
    {
        public string SessionId { get; set; }
        public System.DateTime StartTime { get; set; }
        public int SessionDuration { get; set; }
    }

    public class SessionFinishedResponse
    {
        public string SessionId { get; set; }
        public System.DateTime EndTime { get; set; }
        public int ActualDuration { get; set; }
        public bool WasCompleted { get; set; }
    }

    public class CurrentRunResponse
    {
        public string SessionId { get; set; }
        public System.DateTime StartTime { get; set; }
        public int RemainingSeconds { get; set; }
        public bool IsActive { get; set; }
    }

    public class RunFinishRequest
    {
        public string RunDescription { get; set; }
    }

    public class RunFinishResponse
    {
        public int CompletedSessions { get; set; }
        public int PlannedSessionsAmount { get; set; }
        public int SessionDuration { get; set; }
        public string Description { get; set; }
    }

    public class RunHistoryRequest
    {
        public int Limit { get; set; }
        public int Offset { get; set; }
    }

    public class RunHistoryResponse
    {
        public System.DateTime RunStartTime { get; set; }
        public System.DateTime RunEndTime { get; set; }
        public int CompletedSessions { get; set; }
        public int PlannedSessionsAmount { get; set; }
        public int SessionDuration { get; set; }
        public string Description { get; set; }
    }
}
