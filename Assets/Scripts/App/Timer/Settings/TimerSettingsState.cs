namespace App.Timer.Settings
{
    public class TimerSettingsState
    {
        public int SessionDuration { get; set; }
        public int SessionsAmount { get; set; }

        public bool Equals(TimerSettingsState other)
        {
            if (other == null) return false;

            return SessionDuration == other.SessionDuration && SessionsAmount == other.SessionsAmount;
        }
    }
}