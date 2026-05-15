using System.Threading;
using PT.Tools.Debugging;

namespace App.Timer.States
{
    public class RunState
    {
        public RunStatus RunStatus { get; set; }
        public int SessionDuration { get; set; }
        public int PlannedSessionsAmount { get; set; }
        public int PlannedSessionsAmountCompletedSessions { get; set; }
        
        public bool Equals(RunState other)
        {
            if (other == null) return false;

            bool equals = RunStatus == other.RunStatus &&
                          SessionDuration == other.SessionDuration &&
                          PlannedSessionsAmount == other.PlannedSessionsAmount &&
                          PlannedSessionsAmountCompletedSessions == other.PlannedSessionsAmountCompletedSessions; 
            
            if (equals) DebugManager.Log(DebugCategory.Data, $"Run state equals to {other}");

            return equals;
        }
    }
    
    public enum RunStatus
    {
        None, 
        Idle,
        Active,
    }
}