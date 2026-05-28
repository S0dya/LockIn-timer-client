using System.Collections.Generic;

namespace App.Backend.Models
{
    public class SessionStartResponse
    {
        public System.Guid RunId { get; set; }
        public System.DateTime SessionStartedAt { get; set; }
        public int CompletedSessions { get; set; }
        public int SessionDuration { get; set; }
        public int PlannedSessionsAmount { get; set; }
    }

    public class SessionFinishedResponse
    {
        public System.Guid RunId { get; set; }
        public int CompletedSessions { get; set; }
        public int SessionDuration { get; set; }
        public int PlannedSessionsAmount { get; set; }
    }

    public class CurrentRunResponse
    {
        public int CompletedSessions { get; set; }
        public int PlannedSessionsAmount { get; set; }
        public int SessionDuration { get; set; }
        public System.DateTime? CurrentSessionStartTime { get; set; }
    }

    public class RunFinishRequest
    {
        public string? RunDescription { get; set; }
    }

    public class RunFinishResponse
    {
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public int CompletedSessions { get; set; }
        public int PlannedSessionsAmount { get; set; }
        public int SessionDuration { get; set; }
        public string? Description { get; set; }
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
        public string? Description { get; set; }
    }

    public class CancelSessionResponse
    {
        public System.DateTime StartedAt { get; set; }
        public int SessionProgressionSeconds { get; set; }
    }

    public class CancelRunResponse
    {
        public System.DateTime RunStartTime { get; set; }
        public System.DateTime CancelledAt { get; set; }
        public int CompletedSessions { get; set; }
        public int PlannedSessionsAmount { get; set; }
        public int SessionDuration { get; set; }
    }
}
