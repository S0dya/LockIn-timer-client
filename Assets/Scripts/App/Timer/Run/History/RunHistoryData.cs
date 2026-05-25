namespace App.Timer.Run.History
{
    public class RunHistoryData
    {
        public int CompletedSessions { get; private set; }
        public int PlannedSessionsAmount { get; private set; }
        public int SessionDuration { get; private set; }
        public string Description { get; private set; }
        public System.DateTime RunStartTime { get; private set; }
        public System.DateTime RunEndTime { get; private set; }

        public RunHistoryData(int completedSessions, int plannedSessionsAmount, int sessionDuration, string description, System.DateTime runStartTime, System.DateTime runEndTime)
        {
            CompletedSessions = completedSessions;
            PlannedSessionsAmount = plannedSessionsAmount;
            SessionDuration = sessionDuration;
            Description = description;
            RunStartTime = runStartTime;
            RunEndTime = runEndTime;
        }
    }
}