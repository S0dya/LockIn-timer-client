namespace App.Timer.States
{
    public class RunState
    {
        public RunStatus RunStatus { get; set; }
        public int SessionDuration { get; set; }
        public int PlannedSessionsAmount { get; set; }
        public int PlannedSessionsAmountCompletedSessions { get; set; }
    }
    
    public enum RunStatus
    {
        None, 
        Idle,
        Active,
    }
}