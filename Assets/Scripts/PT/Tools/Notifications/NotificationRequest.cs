using System;

namespace PT.Tools.Notifications
{
    public class NotificationRequest
    {
        public NotificationEnum Enum { get; private set; }
        public NotificationData Data { get; private set; }
        
        public NotificationRequest(NotificationEnum notificationEnum, NotificationData data)
        {
            Enum = notificationEnum;
            Data = data;
        }
    }
}
