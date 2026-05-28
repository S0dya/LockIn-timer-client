using System;

namespace App.Features.Settings
{
    public class TimerSettingsData
    {
        public int DurationIndex { get; private set; }
        public int SessionsAmountIndex { get; private set; }

        public TimerSettingsData(int durationIndex, int sessionsAmountIndex)
        {
            DurationIndex = durationIndex;
            SessionsAmountIndex = sessionsAmountIndex;
        }
    }
}
