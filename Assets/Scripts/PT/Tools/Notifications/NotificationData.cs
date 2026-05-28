using System;
using UnityEngine;

namespace PT.Tools.Notifications
{
    [Serializable]
    public class NotificationData
    {
        public string Title;
        public string Description;
        [Min(0)] public int DelaySeconds;

        public TimeSpan Delay => TimeSpan.FromSeconds(DelaySeconds);

        public NotificationData(string title, string description, int delaySeconds)
        {
            Title = title;
            Description = description;
            DelaySeconds = delaySeconds;
        }
    }
}